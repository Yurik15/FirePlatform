using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirePlatform.Services;
using FirePlatform.WebApi.Model;
using FirePlatform.WebApi.Model.Requests;
using FirePlatform.WebApi.Model.Responses;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services;
using FirePlatform.WebApi.Services.Parser;
using FirePlatform.WebApi.Services.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NCalc;

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

        private (bool fromDB, List<ItemGroup> items) LoadIfExistsInDB(FirePlatform.Models.Models.ScriptDefinition request)
        {
            bool fromDb = false;
            List<ItemGroup> res = null;
            var service = Service.GetMainTemplatesService();
            if (service != null)
            {

                var requestData = new Models.Models.MainTemplates()
                {
                    Lng = request.Lng,
                    LongName = request.LongName,
                    ShortName = request.ShortName
                };

                var foundTmp = service.TryGetTemplateOrDefault(requestData);

                if (foundTmp == null)
                {
                    var downalodedTmp = Download(request);
                    res = Parser.PrepareControls(downalodedTmp);

                    try
                    {
                        var bytes = res.Serialize();
                        if (bytes != null)
                        {
                            requestData.Data = bytes;
                            service.Save(requestData);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                else
                {
                    res = foundTmp.Data?.DeSerialize() as List<ItemGroup>;
                    fromDb = true;
                }
            }
            return (fromDb, res);
        }

        [HttpPost("api/[controller]/LoadTmp")]
        [EnableCors("AllowAll")]
        //[Authorize]
        [AllowAnonymous]
        public async Task<OkObjectResult> Load([FromBody] TemplateModel request)
        {
            CalculationTools.NotFoundName_Visibility = new Dictionary<string, int>();
            List<ItemGroup> res;
            List<Item> savedItems = null;
            var service = Service.GetUserTemplatesService();
            var result = await service.Get(x => x.Name == request.SavedName && x.UserId == request.UserId && x.MainName == request.ShortName);
            if (result != null && result.Count() > 0)
            {
                var tmp = result.FirstOrDefault();
                var tmpData = tmp?.Data?.DeSerialize();
                savedItems = tmpData as List<Item>;
            }

            var scriptDefinitionService = Service.GetScriptDefinitionService();
            var scripts = await scriptDefinitionService.GetAsync();
            var scriptDetails = scripts.FirstOrDefault(x => x.ShortName == request.ShortName);
            var items = LoadIfExistsInDB(scriptDetails);

            if (items.fromDB)
                res = Parser.PrepareControlsLoadedFromDB(items.items, savedItems);
            else
                res = items.items;

            var token = ((Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpRequestHeaders)HttpContext.Request.Headers).HeaderAuthorization;
            SetSessionScript(request.UserId, token.ToString(), request.TemplateGuiID, res);

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
                x.TemplateGuiID = request.TemplateGuiID
            );

            return Ok(res);
        }

        [HttpGet("api/[controller]/FetchPicture")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        [Authorize]
        //[AllowAnonymous]
        public ActionResult FetchPictureForItem(int numberId, int groupId)
        {
            var response = Pictures.FirstOrDefault(x => x.NumID == numberId && x.GroupID == groupId);
            return Ok(response);
        }

        [HttpGet("api/[controller]/Set")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        //[Authorize]
        [AllowAnonymous]
        public async Task<OkObjectResult> Set(int groupId = 0, int itemId = 0, string value = "", int userId = 0, int templateGuiID = 0)
        {
            var res = Tuple.Create<List<ItemGroup>, List<Item>>(null, null);
            try
            {
                var startDate = DateTime.Now;
                var token = ((Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpRequestHeaders)HttpContext.Request.Headers).HeaderAuthorization;
                var UsersTmp = GetSessionScript(userId, token.ToString(), templateGuiID);

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
                    group.IsVisiblePrev = group.IsVisible;
                    group.UpdateGroup();
                    foreach (var item in group.Items)
                    {
                        item.IsVisiblePrev = item.IsVisible;
                        if (group.IsVisible)
                            item.UpdateItem();
                        else if (item.IsVisible)
                            item.IsVisible = false;
                    }
                }

                var resultGroups = new List<ItemGroup>();
                UsersTmp.ForEach(x =>
                {
                    if (x.IsVisible != x.IsVisiblePrev)
                        resultGroups.Add(new ItemGroup()
                        {
                            IndexGroup = x.IndexGroup,
                            IsVisible = x.IsVisible
                        });
                });

                var changedItems = new List<Item>();
                UsersTmp.ForEach(x =>
                {
                    if (x.IsVisible)
                        x.Items?.ForEach(y => changedItems.Add(y));
                });

                changedItems = changedItems.Where(x => x.IsVisible != x.IsVisiblePrev || (x.IsVisible && !string.IsNullOrWhiteSpace(x.Formula))).ToList();
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
            }
            catch (Exception ex)
            {

            }

            //await SaveCustomTemplate(new CustomTamplate() { MainName = "A", Name = "B" });
            return Ok(res);
        }

        [HttpPost("api/[controller]/SaveTemplate")]
        [EnableCors("AllowAll")]
        [AllowAnonymous]
        //[Authorize]
        public OkObjectResult SaveCustomTemplate([FromBody] CustomTamplate template)
        {
            var tmp = GetSessionScript(template.UserId, "TEST", template.TemplateGuiID);
            List<Item> items = new List<Item>();
            foreach (var group in tmp)
            {
                foreach (var item in group.Items)
                {
                    if (item.Value != item.InitialValue && item.IsVisible)
                    {
                        items.Add(item);
                    }
                }
            }
            var bytes = items.Serialize();
            var service = Service.GetUserTemplatesService();
            var result = service.Save(new Models.Models.Users() { Id = template.UserId }, template.MainName, template.Name, bytes);
            return Ok(result.success);
        }

        private string Download(FirePlatform.Models.Models.ScriptDefinition scriptDefinition)
        {
            string file_contents = string.Empty;
            using (var wc = new System.Net.WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;// GetEncoding("Windows-1250"); 
                file_contents = wc.DownloadString(scriptDefinition.Link);
            }
            return file_contents;
        }

        private void SetSessionScript(int userId, string token, int templateGuiID, List<ItemGroup> data)
        {
            if (ItemDataPerUsers == null)
            {
                ItemDataPerUsers = new List<ItemDataPerUser>();
            }

            var userTemplates = ItemDataPerUsers.FirstOrDefault(x => x.UserId == userId);
            if (userTemplates == null)
            {
                userTemplates = new ItemDataPerUser(userId)
                {
                    SessionUserTemplates = new Dictionary<string, List<UserTemplates>>()
                     {
                         { token, new List<UserTemplates>(){ new UserTemplates(templateGuiID) { UsersTmp = data } } }
                     }
                };
                ItemDataPerUsers.Add(userTemplates);
            }
            else
            {
                var sessionTemplates = userTemplates.SessionUserTemplates;
                List<UserTemplates> templates = null;
                if (sessionTemplates.ContainsKey(token))
                {
                    templates = sessionTemplates[token];
                }
                else
                {
                    sessionTemplates.Clear();
                    templates = new List<UserTemplates>();
                    sessionTemplates.Add(token, templates);
                }

                var selectedTemplate = templates.FirstOrDefault(x => x.TemplateGuiID == templateGuiID);
                if (selectedTemplate == null)
                {
                    selectedTemplate = new UserTemplates(templateGuiID) { UsersTmp = data };
                    templates.Add(selectedTemplate);
                }
                else
                {
                    selectedTemplate.UsersTmp = data;
                }
            }
        }
        private List<ItemGroup> GetSessionScript(int userId, string token, int templateGuiID)
        {
            var userTemplates = ItemDataPerUsers?.FirstOrDefault(x => x.UserId == userId);
            if (userTemplates == null)
            {
                return null;
            }

            var sessionTemplates = userTemplates.SessionUserTemplates;
            if (sessionTemplates.Count > 1)
            {
                var needToRemoveOldSession = sessionTemplates.Where(x => x.Key != token);
                foreach (var item in needToRemoveOldSession)
                {
                    sessionTemplates[item.Key].Clear();
                    sessionTemplates.Remove(item.Key);
                }
            }
            List<UserTemplates> templates = null;
            if (sessionTemplates.ContainsKey(token))
            {
                templates = sessionTemplates[token];
            }
            else
            {
                sessionTemplates.Clear();
                return null;
            }
            return templates.FirstOrDefault(x => x.TemplateGuiID == templateGuiID)?.UsersTmp;
        }
    }
}