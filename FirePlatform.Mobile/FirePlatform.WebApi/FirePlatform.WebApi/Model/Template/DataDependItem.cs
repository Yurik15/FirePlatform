using System;
namespace FirePlatform.WebApi.Model.Template
{
    public class DataDependItem
    {
        public string Name { get; set; }
        public bool IsVisibile { get; set; }
        public bool IsGhostFormula { get; set; }
        public _Item ReferencedItem { get; set; }
    }
}
