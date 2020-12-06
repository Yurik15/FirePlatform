using System.Collections.Generic;

namespace FirePlatform.WebApi.Model.Template
{
    public class ItemDataPerUser
    {
        public ItemDataPerUser(int userId)
        {
            UserId = userId;
            SessionUserTemplates = new Dictionary<string, List<UserTemplates>>();
        }
        public int UserId { get; }

        public Dictionary<string, List<UserTemplates>> SessionUserTemplates { get; set; }
    }
}
