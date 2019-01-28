using System;
namespace FirePlatform.WebApi.Model
{
    public class ItemGroup
    {
        public bool Expanded { get; set; }
        public int NumID { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public bool IsVisible { get; set; }
        public Item[] Items { get; set; }
    }
}
