using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services.Tools;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Model
{
    public class ItemGroup
    {
        private string _visCondition = String.Empty;
        private string _title = String.Empty;
        private string _tag = String.Empty;

        public ItemGroup()
        {
            VisConditionNameVaribles = new List<string>();
        }

        public bool Expanded { get; set; }
        public int IndexGroup { get; set; }
        public string Title { get => _title; set => _title = value?.Trim().ToLower() ?? string.Empty; }
        [JsonIgnore]
        public string Tag { get => _tag; set => _tag = value?.Trim().ToLower() ?? string.Empty; }

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
        public List<string> VisConditionNameVaribles { get; set; }

        public bool IsVisible { get; set; }
        public List<Item> Items { get; set; }

        [JsonIgnore]
        public List<KeyValuePair<string, List<DataDependItem>>> DependToItems { get; set; }

        [JsonIgnore]
        public bool InitialVisibility { get; set; }

        // Methods

        /*public void EvaluateVis(ref Dictionary<string, object> variables, ref Dictionary<String, String[,]> datamatrices)
        {
            if (Tag == null) { IsVisible = true; return; }
            if (Tag.Length < 3) { IsVisible = true; return; }

            string expr = Tag.Substring(2).Trim();
            NCalc.Expression expr1 = new NCalc.Expression(expr, NCalc.EvaluateOptions.IgnoreCase);

            foreach (KeyValuePair<string, object> v in variables)
            {
                expr1.Parameters[v.Key] = variables[v.Key];
            }

            ParameterExtractionVisitor visitor = new ParameterExtractionVisitor();
            LogicalExpression expression = NCalc.Expression.Compile(expr.ToLowerInvariant(), false);

            try
            {
                expression.Accept(visitor);
                List<String> extractedParameters = visitor.Parameters.ToList();
            }
            catch (Exception ex)
            {

            }

            foreach (string pn in visitor.Parameters.ToList())
            {
                if (!variables.Keys.Contains(pn.ToLowerInvariant()))
                {
                    expr1.Parameters[pn.ToLowerInvariant()] = null;
                }
            }

            try
            {
                object result = expr1.Evaluate();
                if (result != null && result.GetType() == typeof(bool) && (bool)result == true) { IsVisible = true; return; } else { IsVisible = false; return; }
            }
            catch (Exception ex)
            {
                IsVisible = false;
            }


        }

        */
    }
}
