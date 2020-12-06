using System.Collections.Generic;

namespace FirePlatform.WebApi.Model.Template
{
    public class UserTemplates
    {
        public UserTemplates(int templateGuiID)
        {
            TemplateGuiID = templateGuiID;
        }
        public int TemplateGuiID { get; }
        public List<ItemGroup> UsersTmp { get; set; }
    }
}
