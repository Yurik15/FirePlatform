using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services.Tools;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Model
{
    public class Item
    {
        #region fields
        private string _visCondition = String.Empty;
        private string _formula = String.Empty;
        private string _varibles = String.Empty;
        private string _nameVarible = String.Empty;
        private string _groupTitle;
        private string _title;
        private string _tooltipText;
        private bool _isVisiblePrev;
        private bool _isVisible = true;

        #endregion fields

        public Item()
        {
            GhostFormulas = new List<GhostFormula>();
            ComboItems = new List<ComboItem>();

            VisConditionNameVaribles = new List<string>();
            FormulaNameVaribles = new List<string>();

            DependToItems = new List<KeyValuePair<string, List<DataDependItem>>>();
            DependToItemsForFormulas = new List<KeyValuePair<string, List<DataDependItem>>>();
            NeedNotifyItems = new List<Item>();
            NeedNotifyGroups = new List<ItemGroup>();
        }
        #region properties
        public int GroupID { get; set; }
        public int NumID { get; set; }

        public object Value { get; set; }

        public string Title { get => _title; set => _title = value?.Trim().ToLower() ?? string.Empty; }
        public string Type { get; set; }
        public string GroupTitle { get => _groupTitle; set => _groupTitle = value?.Trim().ToLower() ?? string.Empty; }
        public string NameVarible { get => _nameVarible; set => _nameVarible = value?.Trim().ToLower() ?? string.Empty; }
        public string Dec { get; set; }

        public double Min { get; set; }
        public double Max { get; set; }
        public double Inc { get; set; }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                IsVisiblePrev = _isVisible;
                _isVisible = value;
            }
        }
        public bool IsGroupVisible { get; set; } = true;

        public List<ComboItem> ComboItems { get; set; }
        public Picture Picture { get; set; }
        #endregion properties

        #region ignore fields
        [JsonIgnore]
        public bool IsVisiblePrev
        {
            get => _isVisiblePrev;
            set
            {
                _isVisiblePrev = value;
            }
        }
        [JsonIgnore]
        public string TooltipText { get => _tooltipText; set => _tooltipText = value?.Trim().ToLower() ?? string.Empty; }
        [JsonIgnore]
        public List<GhostFormula> GhostFormulas { get; set; }
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
        [JsonIgnore]
        public List<string> VisConditionNameVaribles { get; set; }
        [JsonIgnore]
        public string Formula
        {
            get => _formula;
            set
            {
                _formula = value?.Trim().ToLower() ?? string.Empty;
                if (!string.IsNullOrEmpty(_formula))
                {
                    FormulaNameVaribles.AddRange(CalculationTools.GetVaribliNames(_formula));
                    FormulaNameVaribles = FormulaNameVaribles.Distinct().ToList();
                }
            }
        }
        [JsonIgnore]
        public List<string> FormulaNameVaribles { get; set; }
        [JsonIgnore]
        public string Varibles
        {
            get
            {
                if (string.IsNullOrEmpty(_varibles))
                {
                    var items = ComboItems;
                    if (items.Any())
                    {
                        var separate = "|";
                        var keys = items.Select(x => string.Join(separate, x.GroupKey));
                        _varibles = string.Join(separate, keys)?.Trim().ToLower() ?? string.Empty;
                    }
                }
                return _varibles;
            }
        }
        [JsonIgnore]
        public ItemGroup ParentGroup { get; set; }
        [JsonIgnore]
        public ModifiedFlag State { get; set; }
        [JsonIgnore]
        public object InitialValue { get; set; }
        [JsonIgnore]
        public List<KeyValuePair<string, List<DataDependItem>>> DependToItems { get; set; }
        [JsonIgnore]
        public List<KeyValuePair<string, List<DataDependItem>>> DependToItemsForFormulas { get; set; }
        [JsonIgnore]
        public List<Item> NeedNotifyItems { get; set; }
        [JsonIgnore]
        public List<ItemGroup> NeedNotifyGroups { get; set; }

        [JsonIgnore]
        public string[,] Matrix { get; set; }

        [JsonIgnore]
        public string NameVaribleMatrix { get; set; } //TODO NEED TO BE REFACTOR
        #endregion ignore fields
    }
    public enum ItemType { Text, Formula, BackCalc, Combo, Num, Check, Hidden, Message, Picture };
    public enum ModifiedFlag { Unchanged, Modified }
}
