﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services.Tools;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Model
{
    [Serializable]
    public class ItemGroup
    {
        private string _visCondition = String.Empty;
        private string _title = String.Empty;
        private string _tag = String.Empty;
        private List<Item> _items;
        [NonSerialized]
        private List<KeyValuePair<string, List<DataDependItem>>> _dependToItems;
        private List<string> _visConditionNameVaribles;
        private bool isVisible;

        public ItemGroup()
        {
            VisConditionNameVaribles = new List<string>();
            Expanded = true;
        }

        public int TemplateGuiID { get; set; }
        public bool Expanded { get; set; }
        public int IndexGroup { get; set; }
        public string Title { get => _title; set => _title = value?.Trim() ?? string.Empty; }
        [JsonIgnore]
        public string Tag { get => _tag; set => _tag = value?.Trim().ToLower() ?? string.Empty; }

        [JsonIgnore]
        public string VisCondition
        {
            get => _visCondition;
            set
            {
                _visCondition = value?.Trim().ToLower() ?? string.Empty;
                Recalculate();
            }
        }
        public List<string> VisConditionNameVaribles { get => _visConditionNameVaribles ?? (_visConditionNameVaribles = new List<string>()); set => _visConditionNameVaribles = value; }

        public bool IsVisible
        {
            get => isVisible; set
            {
                IsVisiblePrev = isVisible;
                isVisible = value;
            }
        }
        [JsonIgnore]
        public bool IsVisiblePrev { get; set; }
        public List<Item> Items { get => _items ?? (_items = new List<Item>()); set => _items = value; }
        [JsonIgnore]
        public List<KeyValuePair<string, List<DataDependItem>>> DependToItems { get => _dependToItems ?? (_dependToItems = new List<KeyValuePair<string, List<DataDependItem>>>()); set => _dependToItems = value; }

        [JsonIgnore]
        public bool InitialVisibility { get; set; }

        public void Recalculate()
        {
            RecalculateVisConditionNameVaribles();
        }
        private void RecalculateVisConditionNameVaribles()
        {
            if (!string.IsNullOrEmpty(_visCondition))
            {
                VisConditionNameVaribles.AddRange(CalculationTools.GetVaribliNames(_visCondition));
                VisConditionNameVaribles = VisConditionNameVaribles.Distinct().ToList();
            }
            else
            {
                VisConditionNameVaribles = null;
            }
        }
    }
}
