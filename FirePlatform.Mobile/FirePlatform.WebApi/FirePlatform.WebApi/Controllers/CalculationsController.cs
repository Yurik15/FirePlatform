using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using FirePlatform.Services;
using FirePlatform.WebApi.Model;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services;
using FirePlatform.WebApi.Services.Parser;
using FirePlatform.WebApi.Services.Tools;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NCalc;

namespace FirePlatform.WebApi.Controllers
{
    [ApiController]
    public class CalculationsController : BaseController
    {
        public static List<ItemGroup> UsersTmp { get; set; }

        readonly ICalculationService _calculationService;
        public CalculationsController(Service service, IMapper mapper, ICalculationService calculationService)
                              : base(service, mapper)
        {
            _calculationService = calculationService;
        }
        [HttpGet("api/[controller]/LoadTmp")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        public OkObjectResult Load(int numberTmpl = 1)
        {
            var content = Download(numberTmpl);

            var res = Parser.PrepareControls(content);

            UsersTmp = res;
            return Ok(res);
        }

        [HttpGet("api/[controller]/test-calc")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
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

        [HttpGet("api/[controller]/Set")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        public OkObjectResult Set(int groupId, int itemId, string value)
        {
            var startDate = DateTime.Now;
            var group = UsersTmp.FirstOrDefault(x => x.IndexGroup == groupId);
            var item = group.Items.FirstOrDefault(x => x.NumID == itemId);
            dynamic newValue = null;
            if (item.Type == ItemType.Num.ToString())
            {
                newValue = double.Parse(value);
            }
            else if (item.Type == ItemType.Check.ToString())
            {
                newValue = bool.Parse(value);
            }
            else if (item.Type == ItemType.Combo.ToString())
            {
                item.NameVarible = value;
                newValue = true;
            }
            item.Value = newValue;
            item.NotifyAboutChange();

            var changedGroup = item.NeedNotifyGroups;
            var changedItems = item.NeedNotifyItems/*.Where(x => !changedGroup.Any(y => y.IndexGroup == x.GroupID))*/.ToList();

            foreach (var needNotifyItem in item.NeedNotifyItems)
            {
                if (needNotifyItem.Type == ItemType.Formula.ToString() || needNotifyItem.Type == ItemType.Hidden.ToString())
                {
                    changedGroup.AddRange(needNotifyItem.NeedNotifyGroups);
                    changedItems.AddRange(needNotifyItem.NeedNotifyItems);
                }
            }
            changedItems = changedItems.Where(x => x.IsVisible || x.IsVisible != x.IsVisiblePrev).ToList();
            (List<ItemGroup>, List<Item>) res = (groups: changedGroup, items: changedItems);

            var result = DateTime.Now - startDate;
            Debug.WriteLine($"[SET VALUE] - Time - minutes : {result.Minutes} or seconds : {result.Seconds}");
            return Ok(res);
        }

        [HttpGet("api/[controller]/LoadTemplates")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        public OkObjectResult LoadTemplatesTest(int countTemplates = 9)
        {
            var templates = new List<Template>();
            for (int i = 1; i <= countTemplates; i++)
            {
                templates.Add(
                    new Template
                    {
                        Id = i,
                        Name = "Template " + i
                    }
                );
            }

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
            }
            return file_contents;
        }
    }
}