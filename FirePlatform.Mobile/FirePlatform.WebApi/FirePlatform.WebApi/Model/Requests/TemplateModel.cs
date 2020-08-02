using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Model.Requests
{
    public class TemplateModel
    {
        public string Lng { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Stage { get; set; }
        public string Type { get; set; }
        public string Topic { get; set; }
        [JsonIgnore]
        public string Link { get; set; }
        public int UserId { get; set; }
        public int TemplateGuiID{ get; set; }
        public bool IsCustom { get; set; }
        public string SavedName { get; set; }
        public List<TemplateModel> SavedTemplates{ get; set; }

        public TemplateModel()
        {
            SavedTemplates = new List<TemplateModel>();
        }
    }
}
