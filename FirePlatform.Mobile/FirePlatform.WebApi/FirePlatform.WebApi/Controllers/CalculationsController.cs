using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using FirePlatform.Services;
using FirePlatform.Utils.AlgorithmHelpers;
using FirePlatform.WebApi.Model;
using FirePlatform.WebApi.Model.Requests;
using FirePlatform.WebApi.Model.Responses;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services;
using FirePlatform.WebApi.Services.Parser;
using FirePlatform.WebApi.Services.Tools;
using LZString;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NCalc;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Controllers
{
    [ApiController]
    public class CalculationsController : BaseController
    {
        readonly ICalculationService _calculationService;
        public static List<ItemDataPerUser> ItemDataPerUsers { get; set; }
        public static List<PictureResponse> Pictures { get; set; }

        public CalculationsController
            (
                Service service,
                IMapper mapper,
                ICalculationService calculationService
            )
                              : base(service, mapper)
        {
            _calculationService = calculationService;
        }
        static CalculationsController()
        {
            ItemDataPerUsers = new List<ItemDataPerUser>();
            Pictures = new List<PictureResponse>();
        }

        [HttpGet("api")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [AllowAnonymous]
        public OkObjectResult Init()
        {
            return Ok("It works");
        }

        [HttpGet("api/[controller]/LoadTmp")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [Authorize]
        public OkObjectResult Load(int numberTmpl = 1, int userId = 0, bool isRightTemplate = false)
        {
            List<ItemGroup> res;
            var content = Download(numberTmpl);

            res = Parser.PrepareControls(content);

            var isExistsUser = ItemDataPerUsers.Any(x => x.UserId == userId);
            if (isExistsUser)
            {
                foreach (var data in ItemDataPerUsers)
                {
                    if (data.UserId == userId)
                    {
                        if (isRightTemplate)
                        {
                            data.UsersTmpRight = res;
                        }
                        else
                        {
                            data.UsersTmpLeft = res;  
                        }
                        break;
                    }
                }
            }
            else
            {
                var itemDataPerUser = isRightTemplate ? new ItemDataPerUser
                {
                    UserId = userId,
                    UsersTmpRight = res
                } :
                new ItemDataPerUser
                {
                    UserId = userId,
                    UsersTmpLeft = res
                };

                ItemDataPerUsers.Add(itemDataPerUser);
            }
            List<Item> ite = new List<Item>();
            foreach (var i in res)
            {
                ite.AddRange(i.Items.Where(x => x.IsVisible).ToList());
            }

            foreach (var group in res)
            {
                foreach (var item in group?.Items)
                    if (item != null && item.Picture?.Data != null)
                    {
                        Pictures.Add(new PictureResponse()
                        {
                            NumID = item.NumID,
                            Picture = new Picture()
                            {
                                Data = item.Picture.Data,
                                Name = item.Picture.Name,
                                Id = item.Picture.Id
                            },
                            GroupID = item.GroupID,
                        });
                        item.Picture.Data = null;
                        item.Picture.ToFetch = true;
                    }
            }
            res.ForEach(x =>
                x.IsRightTemplate = isRightTemplate
            );

            return Ok(res);
        }

        [HttpGet("api/[controller]/ClearTemplates")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [Authorize]
        public ActionResult ClearTemplateDataPerUser(int userId)
        {
            //var existingDataperUser = ItemDataPerUsers.FirstOrDefault(x => x.UserId == userId);
            //if (existingDataperUser != null)
            //    existingDataperUser.UsersTmp = new List<ItemGroup>();

            return Ok();
        }

        [HttpGet("api/[controller]/FetchPicture")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [Authorize]
        public ActionResult FetchPictureForItem(int numberId, int groupId)
        {
            var response = Pictures.FirstOrDefault(x => x.NumID == numberId && x.GroupID == groupId);
            return Ok(response);
        }

        [HttpGet("api/[controller]/test-calc")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [Authorize]
        public OkObjectResult TestCalc()
        {
            var parameters = new Dictionary<string, object>()
            {
                { "a", null },
                { "b", null },
                { "c", true },
                { "d", false },
                { "e", 1 },
                {"f", "sdasdasd" }
            };
            a("c || d", parameters);
            a("a && b", parameters);
            a("a || b", parameters);
            a("a && c", parameters);
            a("a || c", parameters);
            a("a && e<=1", parameters);
            a("a>=1 && e<=1", parameters);
            a("a >= 0", parameters);
            a("f == a", parameters);
            a("'a' == a", parameters);
            a("'a' == f", parameters);
            void a(string formula, Dictionary<string, object> param)
            {
                try
                {
                    var expression = new Expression(formula, EvaluateOptions.IgnoreCase)
                    {
                        Parameters = parameters
                    };
                    expression.Evaluate();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"{formula} - {ex.Message} \n");
                }
            }
            return Ok(true);
        }

        [HttpPost("api/[controller]/Preselection")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [Authorize]
        public OkObjectResult Preselection([FromBody] PreselectionRequest request)
        {
            var UsersTmp = request.IsRightTemplate ?
                                   ItemDataPerUsers.FirstOrDefault(x => x.UserId == request.UserId).UsersTmpRight :
                                   ItemDataPerUsers.FirstOrDefault(x => x.UserId == request.UserId).UsersTmpLeft;

            foreach (var group in UsersTmp)
            {
                foreach(var item in group.Items)
                {
                    if(item.Type == ItemType.Combo.ToString())
                    {
                        if (request.PreselectionEnabled)
                        {
                            if (string.IsNullOrWhiteSpace(item.Value as string))
                            {
                                item.Value = item.ComboItems?.FirstOrDefault().GroupKey;
                            }
                        }
                        else
                        {
                            item.Value = null;
                        }
                    }
                }
            }

            foreach (var group in UsersTmp)
            {
                group.UpdateGroup();
                foreach (var item in group.Items)
                {
                    if (group.IsVisible)
                        item.UpdateItem();
                    else
                        item.IsVisible = false;
                }
            }
            var resultGroups = new List<ItemGroup>();
            UsersTmp.ForEach(x => resultGroups.Add(new ItemGroup()
            {
                IndexGroup = x.IndexGroup,
                IsVisible = x.IsVisible
            }));

            var changedItems = new List<Item>();
            UsersTmp.ForEach(x => x.Items?.ForEach(y => changedItems.Add(y)));

            changedItems = changedItems.Where(x => x.IsVisible || x.IsVisible != x.IsVisiblePrev || !string.IsNullOrWhiteSpace(x.Formula)).ToList();
            (List<ItemGroup>, List<Item>) res = (groups: resultGroups, items: changedItems);
            return Ok(res);
        }
        [HttpGet("api/[controller]/Set")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [Authorize]
        public OkObjectResult Set(int groupId = 0, int itemId = 0, string value = "", int userId = 0, bool isRightTemplate = false)
        {
            var res = Tuple.Create<List<ItemGroup>, List<Item>>(null, null);
            try
            {
                var startDate = DateTime.Now;
                var UsersTmp = isRightTemplate ? ItemDataPerUsers.FirstOrDefault(x => x.UserId == userId).UsersTmpRight :
                                                 ItemDataPerUsers.FirstOrDefault(x => x.UserId == userId).UsersTmpLeft;
                var selectedGroup = UsersTmp.FirstOrDefault(x => x.IndexGroup == groupId);
                var selectedItem = selectedGroup.Items.FirstOrDefault(x => x.NumID == itemId);
                object newValue = null;
                if (selectedItem.Type == ItemType.Num.ToString())
                {
                    newValue = double.Parse(value);
                }
                else if (selectedItem.Type == ItemType.Check.ToString())
                {
                    newValue = bool.Parse(value);
                }
                else if (selectedItem.Type == ItemType.Combo.ToString())
                {
                    selectedItem.NameVarible = value;
                    newValue = true;
                }
                selectedItem.Value = newValue;

                foreach (var group in UsersTmp)
                {
                    group.UpdateGroup();
                    foreach (var item in group.Items)
                    {
                        if (group.IsVisible)
                            item.UpdateItem();
                        else
                            item.IsVisible = false;
                    }
                }

                var resultGroups = new List<ItemGroup>();
                UsersTmp.ForEach(x => resultGroups.Add(new ItemGroup()
                {
                    IndexGroup = x.IndexGroup,
                    IsVisible = x.IsVisible
                }));
                
                var changedItems = new List<Item>();
                UsersTmp.ForEach(x => x.Items?.ForEach(y => changedItems.Add(y)));

                changedItems = changedItems.Where(x => x.IsVisible || x.IsVisible != x.IsVisiblePrev || !string.IsNullOrWhiteSpace(x.Formula)).ToList();
                res = Tuple.Create<List<ItemGroup>, List<Item>>(resultGroups, changedItems); ;


                // var result = DateTime.Now - startDate;
                // Debug.WriteLine($"[SET VALUE] - Time - minutes : {result.Minutes} or seconds : {result.Seconds}");
                foreach (var item in res.Item2)
                {
                    if (item.Picture?.Data != null)
                    {
                        Pictures.Add(new PictureResponse()
                        {
                            NumID = item.NumID,
                            Picture = new Picture()
                            {
                                Data = item.Picture.Data,
                                Name = item.Picture.Name,
                                Id = item.Picture.Id
                            },
                            GroupID = item.GroupID,
                        });
                    }
                }

                //foreach (var item in res.Item2)
                //{
                //    if (item.Picture != null && item.Picture.Data != null)
                //    {
                //        item.Picture.Data = null;
                //        item.Picture.ToFetch = true;
                //    }
                //}
                //var itemss = res.Item2.Where(x => x.Picture != null && x.Picture.Data != null);
            }
            catch (Exception ex)
            {

            }

            return Ok(res);
        }

        [HttpGet("api/[controller]/LoadTemplates")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [Authorize]
        public OkObjectResult LoadTemplatesTest()
        {
            var templates = new List<Template>()
            {
                new Template()
                {
                    Id = 1,
                    Name = "General 1"
                },
                new Template()
                {
                    Id = 2,
                    Name = "General 2"
                },
                new Template()
                {
                    Id = 3,
                    Name = "General 3"
                },
                new Template()
                {
                    Id = 4,
                    Name = "Piotrek 1"
                },
                new Template()
                {
                    Id = 5,
                    Name = "Piotrek 2"
                },
                new Template()
                {
                    Id = 6,
                    Name = "Tomek 1"
                },
                new Template()
                {
                    Id = 7,
                    Name = "Tomek 2"
                },
                new Template()
                {
                    Id = 8,
                    Name = "Bartek 1"
                },
                new Template()
                {
                    Id = 9,
                    Name = "Bartek 2"
                },
                new Template()
                {
                    Id = 10,
                    Name = "Tryskacze EN"
                },
                new Template()
                {
                    Id = 11,
                    Name = "Wybuchy EN"
                },
                new Template()
                {
                    Id = 12,
                    Name = "Wybuchy PN"
                },
                new Template()
                {
                    Id = 13,
                    Name = "Warunki tech"
                },
                new Template()
                {
                    Id = 14,
                    Name = "Oddymianie NFPA 204"
                },
                new Template()
                {
                    Id = 15,
                    Name = "Oddymianie PN"
                },
                new Template()
                {
                    Id = 16,
                    Name = "Obciążenie PN"
                },
                new Template()
                {
                    Id = 17,
                    Name = "Obciążenie Eurokod"
                },
                new Template()
                {
                    Id = 18,
                    Name = "Wycena tryskaczy"
                },
                new Template()
                {
                    Id = 19,
                    Name = "Tryskacze NFPA 13"
                },
            };
            return Ok(templates);
        }

        private string Download(int numberTmpl = 1)
        {
            string file_contents = string.Empty;
            using (var wc = new System.Net.WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;// GetEncoding("Windows-1250"); 

                if (numberTmpl == 1) file_contents = wc.DownloadString("https://onedrive.live.com/download.aspx?cid=9214918BD14C3E0C&resid=9214918BD14C3E0C%21771&authkey=ANErohHGOIz32s0");
                if (numberTmpl == 2) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/1/d/1sPt8y5Qi8DyD2kcwr2hi7HdcmYGd0j41wzXSGPgOw-A/export?format=tsv&id=1sPt8y5Qi8DyD2kcwr2hi7HdcmYGd0j41wzXSGPgOw-A&gid=1727084202");
                if (numberTmpl == 3) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/1/d/11ysWSQWAW8KrCJMSDyXXu0dpNxWiDPqMzH8Tk0MJ1aE/export?format=tsv&id=11ysWSQWAW8KrCJMSDyXXu0dpNxWiDPqMzH8Tk0MJ1aE&gid=0");
                if (numberTmpl == 4) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/1/d/1lJ2jBcTE8hKF4zyKYzAXIjPFwf1sJkBWFw2Je_5PD7I/export?format=tsv&id=1lJ2jBcTE8hKF4zyKYzAXIjPFwf1sJkBWFw2Je_5PD7I&gid=0");
                if (numberTmpl == 5) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/1/d/1NEy0c9-fLpjxOeAsFIqM9LlmAYBb9b3hL29-NLke2Cs/export?format=tsv&id=1NEy0c9-fLpjxOeAsFIqM9LlmAYBb9b3hL29-NLke2Cs&gid=0");
                if (numberTmpl == 6) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/1/d/1WXWSW37aglC0O2YDI-ZyADuIrhBusOMPWw8752Rjs_M/export?format=tsv&id=1WXWSW37aglC0O2YDI-ZyADuIrhBusOMPWw8752Rjs_M&gid=0");
                if (numberTmpl == 7) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/1/d/18P5QrPytLq3c7q8epN-spVxmxDcfx-UEPIxqX7kXdqo/export?format=tsv&id=18P5QrPytLq3c7q8epN-spVxmxDcfx-UEPIxqX7kXdqo&gid=0");
                if (numberTmpl == 8) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/1/d/1eP5Q_S5mYm-JBOIiFqTtnQlAm9yidRJjFQNmCP99XFc/export?format=tsv&id=1eP5Q_S5mYm-JBOIiFqTtnQlAm9yidRJjFQNmCP99XFc&gid=0");
                if (numberTmpl == 9) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/1/d/125E669P25ayUVZb5AWmo6_pqriPICyOOpzg_po-yBno/export?format=tsv&id=125E669P25ayUVZb5AWmo6_pqriPICyOOpzg_po-yBno&gid=0");

                if (numberTmpl == 10) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1r2-_StOCfyIhlop2kh8vzJ1DkC8TSbuQauojJ3YNTto/export?format=tsv&id=1r2-_StOCfyIhlop2kh8vzJ1DkC8TSbuQauojJ3YNTto&gid=0");
                if (numberTmpl == 11) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1EUzWym7X68nqdNmDu4KMYpFzGed31XE_JRKXJr6cmB0/export?format=tsv&id=1EUzWym7X68nqdNmDu4KMYpFzGed31XE_JRKXJr6cmB0&gid=0");
                if (numberTmpl == 12) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/17gi9G5T9ogcPxVm5rbpJ4RjPHZ7MUeFv0N4bvu9ej9k/export?format=tsv&id=17gi9G5T9ogcPxVm5rbpJ4RjPHZ7MUeFv0N4bvu9ej9k&gid=0");
                if (numberTmpl == 13) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1FMQB2a9hMIXXkeaEq_I_A0UfDVa-xF042yF9fOuVTeQ/export?format=tsv&id=1FMQB2a9hMIXXkeaEq_I_A0UfDVa-xF042yF9fOuVTeQ&gid=0");
                if (numberTmpl == 14) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1dbGYaP9goc6dC4r02aX8tCfkhq-VnULmjannN2I0E3A/export?format=tsv&id=1dbGYaP9goc6dC4r02aX8tCfkhq-VnULmjannN2I0E3A&gid=0");
                if (numberTmpl == 15) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1jz8zEvtq4RW2vxv0brj9CSFeR3_Mo7-jrUUP35C4pr8/export?format=tsv&id=1jz8zEvtq4RW2vxv0brj9CSFeR3_Mo7-jrUUP35C4pr8&gid=0");
                if (numberTmpl == 16) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1Lv_oVECzgj-QZrDmM3AlN_eoSTglT8aS7LwYmwJbloQ/export?format=tsv&id=1Lv_oVECzgj-QZrDmM3AlN_eoSTglT8aS7LwYmwJbloQ&gid=0");
                if (numberTmpl == 17) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1MYvl9Ybzh3BQ1snIk6PdgKb7PIozMuhOSg_nE9MUBKo/export?format=tsv&id=1MYvl9Ybzh3BQ1snIk6PdgKb7PIozMuhOSg_nE9MUBKo&gid=0");
                if (numberTmpl == 18) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1v6ekGRrzEZjOVc4FJG97nUuGHnVaVMWtuleJOC0zSfo/export?format=tsv&id=1v6ekGRrzEZjOVc4FJG97nUuGHnVaVMWtuleJOC0zSfo&gid=0");
                if (numberTmpl == 19) file_contents = wc.DownloadString("https://docs.google.com/spreadsheets/u/0/d/1aOMzcXAl8kSuVJST-_GbEIx2lQ7tWOpcto8AGzg_vm8/export?format=tsv&id=1aOMzcXAl8kSuVJST-_GbEIx2lQ7tWOpcto8AGzg_vm8&gid=0");

            }
            return file_contents;
        }
    }
}