using System;
using System.Collections.Generic;
using FirePlatform.WebApi.Services.Tools;

namespace FirePlatform.WebApi.Model.Template
{
    public class GhostFormula
    {
        public GhostFormula()
        {
            ConditionsNameVaribles = new List<string>();
        }
        public string Name { get; set; }

        private string _conditions = String.Empty;
        public string Conditions
        {
            get => _conditions;
            set
            {
                _conditions = value;
                if (!string.IsNullOrEmpty(_conditions))
                {
                    ConditionsNameVaribles.AddRange(CalculationTools.GetVaribliNames(_conditions));
                }
            }
        }
        public List<string> ConditionsNameVaribles { get; set; }

        public string Tag { get; set; }
    }
}
