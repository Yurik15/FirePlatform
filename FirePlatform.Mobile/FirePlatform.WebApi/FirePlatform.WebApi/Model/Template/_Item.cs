using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Model
{
    public class _Item
    {
        public _Item()
        {
            GhostFormulas = new List<string>();
            MultiItemTags = new List<string>();
            MultiItemTitles = new List<string>();
            MultiItemDict = new List<MyComboItem>();
            //RelatedItems = new List<_Item>();
            //RelatedGroups = new List<_ItemGroup>();

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
        public string VisCondition { get; set; }
        public string Formula { get; set; }
        public dynamic Value { get; set; }
        public string NameVarible { get; set; } = "";
        public string Varibles { get; set; }


        public double Min { get; set; }
        public double Max { get; set; }
        public double Inc { get; set; }
        public string Dec { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsGroupVisible { get; set; } = true;

        public List<string> GhostFormulas { get; set; }
        public List<string> MultiItemTags { get; set; }
        public List<string> MultiItemTitles { get; set; }
        public List<MyComboItem> MultiItemDict { get; set; }

        [JsonIgnore]
        public List<_Item> DependToItems { get; set; }
        [JsonIgnore]
        public List<_Item> NeedNotifyItems { get; set; }
        [JsonIgnore]
        public List<_ItemGroup> NeedNotifyGroups { get; set; }

        //[JsonIgnore]
        //public List<_Item> RelatedItems { get; set; }
        //[JsonIgnore]
        //public List<_ItemGroup> RelatedGroups { get; set; }
    }
    public enum ItemType { Text, Formula, BackCalc, Combo, Num, Check, Hidden, Message, Picture };
}
