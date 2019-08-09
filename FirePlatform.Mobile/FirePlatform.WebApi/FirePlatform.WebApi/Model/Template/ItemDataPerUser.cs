using System.Collections.Generic;

namespace FirePlatform.WebApi.Model.Template
{
    public class ItemDataPerUser
    {
        public int UserId { get; set; }
        public List<ItemGroup> UsersTmp { get; set; }
    }
}
