using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FirePlatform.WebApi.Services.Tools;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Model.Template
{
    [Serializable]
    public class ComboItem
    {
        private string _displayName = String.Empty;
        private string _groupKey = String.Empty;

        public string DisplayName { get => _displayName; set => _displayName = value?.Trim() ?? string.Empty; }
        public string GroupKey { get => _groupKey; set => _groupKey = value?.Trim().ToLower() ?? string.Empty; }
        public bool IsVisible { get; set; }

        public ComboItem()
        {
            _visConditionNameVaribles = new List<string>();
            IsVisible = true;
        }
        private List<string> _visConditionNameVaribles;
        [JsonIgnore]
        public List<string> VisConditionNameVaribles
        {
            get => _visConditionNameVaribles ?? (_visConditionNameVaribles = new List<string>());
            set => _visConditionNameVaribles = value;
        }
        private string _visCondition = String.Empty;
        [JsonIgnore]
        public string VisCondition
        {
            get => _visCondition;
            set
            {
                _visCondition = value?.Trim().ToLower() ?? string.Empty;
                if (!string.IsNullOrEmpty(_visCondition))
                {
                    VisConditionNameVaribles.AddRange(CalculationTools.GetVaribliNames(_visCondition));
                    VisConditionNameVaribles = VisConditionNameVaribles.Distinct().ToList();
                }
            }
        }

        [NonSerialized]
        private List<KeyValuePair<string, List<DataDependItem>>> _dependToItems;
        [JsonIgnore]
        public List<KeyValuePair<string, List<DataDependItem>>> DependToItems { get => _dependToItems ?? (_dependToItems = new List<KeyValuePair<string, List<DataDependItem>>>()); set => _dependToItems = value; }
    }
}
