using System.Collections.Generic;

namespace FirePlatform.WebApi.Model.Template
{
    public class ItemDataPerUser
    {
        public ItemDataPerUser()
        {
            //inits three user templates by default
            UserTemplates = new List<UserTemplates>()
            {
                new UserTemplates()
                {
                    TemplateGuiID = 0
                },
                new UserTemplates()
                {
                    TemplateGuiID = 1
                },
                 new UserTemplates()
                {
                    TemplateGuiID = 2
                }
            };
        }
        public int UserId { get; set; }
        public List<UserTemplates> UserTemplates { get; set; }
    }
}
