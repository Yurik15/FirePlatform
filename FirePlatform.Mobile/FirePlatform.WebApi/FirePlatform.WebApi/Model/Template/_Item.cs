using System;
using System.Collections.Generic;
using System.Linq;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services.Tools;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Model
{
    public class _Item
    {
        public _Item()
        {
            GhostFormulas = new List<GhostFormula>();
            ComboItems = new List<ComboItem>();

            VisConditionNameVaribles = new List<string>();
            FormulaNameVaribles = new List<string>();

            DependToItems = new List<_Item>();
            NeedNotifyItems = new List<_Item>();
            NeedNotifyGroups = new List<_ItemGroup>();
        }
        public int GroupID { get; set; }
        public string GroupTitle { get; set; }
        public int NumID { get; set; }
        public string Title { get; set; }
        public string TooltipText { get; set; }
        public string Type { get; set; }

        private string _visCondition = String.Empty;
        public string VisCondition
        {
            get => _visCondition;
            set
            {
                _visCondition = value;
                if (!string.IsNullOrEmpty(_visCondition))
                {
                    VisConditionNameVaribles.AddRange(CalculationTools.GetVaribliNames(_visCondition));
                }
            }
        }
        public List<string> VisConditionNameVaribles { get; set; }

        private string _formula = String.Empty;
        public string Formula
        {
            get => _formula;
            set
            {
                _formula = value;
                if (!string.IsNullOrEmpty(_formula))
                {
                    FormulaNameVaribles.AddRange(CalculationTools.GetVaribliNames(_formula));
                }
            }
        }
        public List<string> FormulaNameVaribles { get; set; }

        public dynamic Value { get; set; }
        public string NameVarible { get; set; } = "";

        private string _varibles = "";
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
                        _varibles = string.Join(separate, keys);
                    }
                }
                return _varibles;
            }
        }


        public double Min { get; set; }
        public double Max { get; set; }
        public double Inc { get; set; }
        public string Dec { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsGroupVisible { get; set; } = true;

        public List<GhostFormula> GhostFormulas { get; set; }

        public List<ComboItem> ComboItems { get; set; }

        [JsonIgnore]
        public _ItemGroup ParentGroup { get; set; }
        [JsonIgnore]
        public ModifiedFlag State { get; set; }
        [JsonIgnore]
        public dynamic InitialValue { get; set; }
        [JsonIgnore]
        public List<_Item> DependToItems { get; set; }
        [JsonIgnore]
        public List<_Item> NeedNotifyItems { get; set; }
        [JsonIgnore]
        public List<_ItemGroup> NeedNotifyGroups { get; set; }
    }
    public enum ItemType { Text, Formula, BackCalc, Combo, Num, Check, Hidden, Message, Picture };
    public enum ModifiedFlag { Unchanged, Modified }
}
