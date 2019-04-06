using System;
using System.ComponentModel;

namespace FirePlatform.WebApi.Model.Template
{
    public class ComboItem
    {
        private string _displayName = String.Empty;
        private string _groupKey = String.Empty;

        public string DisplayName { get => _displayName; set => _displayName = value?.Trim().ToLower() ?? string.Empty; }
        public string GroupKey { get => _groupKey; set => _groupKey = value?.Trim().ToLower()??string.Empty; }

        public string[] Keys
        {
            get => GroupKey?.Split(',');
        }
        public bool IsVisible { get; set; } = true;
    }
}
