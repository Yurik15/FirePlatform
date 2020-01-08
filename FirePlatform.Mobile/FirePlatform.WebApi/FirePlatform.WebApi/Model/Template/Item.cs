using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services.Tools;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Model
{
    [Serializable]
    public class Item
    {
        #region fields
        [NonSerialized]
        private string _visCondition = String.Empty;
        [NonSerialized]
        private string _formula = String.Empty;
        [NonSerialized]
        private string _varibles = String.Empty;
        private string _nameVarible = String.Empty;
        private string _groupTitle;
        private string _title;
        [NonSerialized]
        private string _tooltipText;
        [NonSerialized]
        private bool _isVisiblePrev;
        private bool _isVisible = true;
        [NonSerialized]
        private List<ComboItem> _comboItems;
        [NonSerialized]
        private List<GhostFormula> _ghostFormulas;
        [NonSerialized]
        private string _nameVaribleMatrix;
        [NonSerialized]
        private string[,] _matrix;
        [NonSerialized]
        private List<ItemGroup> _needNotifyGroups;
        [NonSerialized]
        private List<Item> _needNotifyItems;
        [NonSerialized]
        private List<KeyValuePair<string, List<DataDependItem>>> _dependToItemsForFormulas;
        [NonSerialized]
        private List<KeyValuePair<string, List<DataDependItem>>> _dependToItems;
        [NonSerialized]
        private ItemGroup _parentGroup;
        [NonSerialized]
        private List<string> _formulaNameVaribles;
        [NonSerialized]
        private List<string> _visConditionNameVaribles;

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
            get
            {
                if(Type == ItemType.HTML.ToString())
                {
                    var parentVis = ParentHtmlItem == null ? true : ParentHtmlItem.IsVisible;
                    return parentVis && _isVisible;
                }
               return _isVisible;
            }
            set
            {
                IsVisiblePrev = _isVisible;
                _isVisible = value;
            }
        }
        public bool IsGroupVisible { get; set; } = true;

        public List<ComboItem> ComboItems { get => _comboItems; set => _comboItems = value; }
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
        public List<GhostFormula> GhostFormulas { get => _ghostFormulas; set => _ghostFormulas = value; }
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
        public List<string> VisConditionNameVaribles { get => _visConditionNameVaribles; set => _visConditionNameVaribles = value; }
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
        public List<string> FormulaNameVaribles { get => _formulaNameVaribles; set => _formulaNameVaribles = value; }
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
        public ItemGroup ParentGroup { get => _parentGroup; set => _parentGroup = value; }
        [JsonIgnore]
        public ModifiedFlag State { get; set; }
        [JsonIgnore]
        public object InitialValue { get; set; }
        [JsonIgnore]
        public List<KeyValuePair<string, List<DataDependItem>>> DependToItems { get => _dependToItems; set => _dependToItems = value; }
        [JsonIgnore]
        public List<KeyValuePair<string, List<DataDependItem>>> DependToItemsForFormulas { get => _dependToItemsForFormulas; set => _dependToItemsForFormulas = value; }
        [JsonIgnore]
        public List<Item> NeedNotifyItems { get => _needNotifyItems; set => _needNotifyItems = value; }
        [JsonIgnore]
        public List<ItemGroup> NeedNotifyGroups { get => _needNotifyGroups; set => _needNotifyGroups = value; }

        [JsonIgnore]
        public string[,] Matrix { get => _matrix; set => _matrix = value; }

        [JsonIgnore]
        public string NameVaribleMatrix { get => _nameVaribleMatrix; set => _nameVaribleMatrix = value; } //TODO NEED TO BE REFACTOR

        #region HTML - PRZEPISY
        [JsonIgnore]
        public Item ParentHtmlItem { get; set; }
        [JsonIgnore]
        public int HtmlLevel { get; set; } = -1;
        #endregion HTML - PRZEPISY

        #endregion ignore fields
    }
    public enum ItemType { Text, Formula, BackCalc, Combo, Num, Check, Hidden, Message, Picture, HTML };
    public enum ModifiedFlag { Unchanged, Modified }
}
