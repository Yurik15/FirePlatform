using System.Collections.Generic;

namespace FirePlatform.Models.Models
{
    public class ItemGroup
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string CustomCSS { get; set; }
        public List<Item> Items { get; set; }
    }
}
