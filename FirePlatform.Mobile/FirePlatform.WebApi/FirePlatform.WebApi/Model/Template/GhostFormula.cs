using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FirePlatform.WebApi.Services.Tools;

namespace FirePlatform.WebApi.Model.Template
{
    [Serializable]
    public class GhostFormula
    {
        private string _conditions = String.Empty;
        private string _name = String.Empty;
        [NonSerialized]
        private List<DataDependItem> dependToItems;
        private List<string> conditionsNameVaribles;

        public GhostFormula()
        {
            ConditionsNameVaribles = new List<string>();
            DependToItems = new List<DataDependItem>();
        }
        public string Name { get => _name; set => _name = value?.Trim().ToLower() ?? string.Empty; }

        public string Conditions
        {
            get => _conditions;
            set
            {
                _conditions = value?.Trim().ToLower() ?? string.Empty;
                if (!string.IsNullOrEmpty(_conditions))
                {
                    ConditionsNameVaribles.AddRange(CalculationTools.GetVaribliNames(_conditions));
                    ConditionsNameVaribles = ConditionsNameVaribles.Distinct().ToList();
                }
            }
        }
        public List<DataDependItem> DependToItems { get => dependToItems ?? (dependToItems = new List<DataDependItem>()); set => dependToItems = value; }
        public List<string> ConditionsNameVaribles { get => conditionsNameVaribles ?? (conditionsNameVaribles = new List<string>()); set => conditionsNameVaribles = value; }

        public string Tag { get; set; }
    }
}
