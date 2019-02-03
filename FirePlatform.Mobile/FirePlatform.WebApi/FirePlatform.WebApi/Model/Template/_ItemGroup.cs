using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace FirePlatform.WebApi.Model
{
    public class _ItemGroup
    {
        public bool Expanded { get; set; }
        public int IndexGroup { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public string VisCondition { get; set; }
        public bool IsVisible { get; set; }
        public List<_Item> Items { get; set; }

        public List<_Item> DependToItems { get; set; }

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
