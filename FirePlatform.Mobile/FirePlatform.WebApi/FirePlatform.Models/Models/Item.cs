using FirePlatform.Models.Enums;

namespace FirePlatform.Models.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDisabled { get; set; }
        public ItemInputTypes InputType { get; set; }
        public string TooltipText { get; set; }
        public dynamic Value { get; set; }
        public string CustomCSS { get; set; }
        public int ParentId { get; set; }
    }
}
