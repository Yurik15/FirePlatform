using System;
using System.Xml.Serialization;

namespace FirePlatform.Mobile.Common.Entities
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
