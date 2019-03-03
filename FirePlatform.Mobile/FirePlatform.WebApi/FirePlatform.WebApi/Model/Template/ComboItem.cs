using System;
namespace FirePlatform.WebApi.Model.Template
{
    public class ComboItem
    {
        public string DisplayName { get; set; }
        public string GroupKey { get; set; }
        public string[] Keys
        {
            get => GroupKey?.Split(',');
        }
        public bool IsVisible { get; set; } = true;
    }
}
