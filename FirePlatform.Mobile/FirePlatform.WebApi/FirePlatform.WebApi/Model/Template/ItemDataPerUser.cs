using System.Collections.Generic;

namespace FirePlatform.WebApi.Model.Template
{
    public class ItemDataPerUser
    {
        public int UserId { get; set; }
        public List<ItemGroup> UsersTmpLeft { get; set; }
        public List<ItemGroup> UsersTmpRight { get; set; }
    }
}
