using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NCalc;

namespace FirePlatform.WebApi.Services.Tools
{
    public static class CalculationTools
    {
        public static Dictionary<string, int> NotFoundName_Visibility = new Dictionary<string, int>();
        public static Dictionary<string, int> NotFoundName_Formula = new Dictionary<string, int>();

        public static bool? CalculateVis(string formula, Dictionary<string, object> parameters)
        {
            try
            {
                Expression expression = new Expression(formula, EvaluateOptions.IgnoreCase)
                {
                    Parameters = parameters
                };
                return Convert.ToBoolean(expression.Evaluate());
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine($"VISIBILITY: {formula}; PARAMS - {string.Join('|', parameters.Select(x=> $"key - {x.Key}, value - {x.Value}").ToList())}; exception:{ex.Message} \n");
                if (!NotFoundName_Visibility.ContainsKey(ex.Message))
                {
                    NotFoundName_Visibility.Add(ex.Message, 1);
                }
                else
                {
                    NotFoundName_Visibility[ex.Message] += 1;
                }
                //System.Diagnostics.Debug.WriteLine($"[VISIBILITY] {formula} - {ex.Message} \n");
                return null;
            }
            return default(bool);
        }

        public static object CalculateFormulasMatrix(string[,] matrix, Dictionary<string, object> parameters)
        {
            try
            {
                if(parameters.Count == 1)
                {
                    var value = parameters.First().Value;
                    for (int i = 0; i < matrix.Length -1; i++)
                    {
                        if(i % 2 == 0)
                        {

                        }
                    }
                }
                
                /* Expression expression = new Expression("", EvaluateOptions.IgnoreCase)
                 {
                     Parameters = parameters
                 };
                 var resultEvaluate = expression.Evaluate();
                 return resultEvaluate;*/
                return null;
            }
            catch (Exception ex)
            {
                if (!NotFoundName_Formula.ContainsKey(ex.Message))
                {
                    NotFoundName_Formula.Add(ex.Message, 1);
                }
                else
                {
                    NotFoundName_Formula[ex.Message] += 1;
                }
                return null;
            }
        }
        public static object CalculateFormulas(string formula, Dictionary<string, object> parameters)
        {
            try
            {
                if ("ns*k*(pow (p,0.5)) #2 dm³/min".Equals(formula))
                {
                    formula = "ns*k*(pow (p,0.5))";
                }
                Expression expression = new Expression(formula, EvaluateOptions.IgnoreCase)
                {
                    Parameters = parameters
                };
                var resultEvaluate = expression.Evaluate();
                return resultEvaluate;
            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine($"[CalculateFormulas] {formula} - {ex.Message} \n");
                if (!NotFoundName_Formula.ContainsKey(ex.Message))
                {
                    NotFoundName_Formula.Add(ex.Message, 1);
                }
                else
                {
                    NotFoundName_Formula[ex.Message] += 1;
                }
                return null;
            }
            return default(bool);
        }

        public static List<string> GetVaribliNames(string condition)
        {
            var names = new List<string>();

            condition = condition.Replace(" or ", "||").Replace(" and ", "&&");

            if (string.IsNullOrEmpty(condition)) return names;
            //Debug.WriteLine($"ITEM VISIBILITY : {condition}");

            char[] splitChars = new char[] { '&', '|', '!', '(', ')', '=', '<', '>', ',', '?', '-', '+', '/', '*', '#' };
            var parts = condition.Split(splitChars).ToList();
            if (parts.Any())
            {
                var result = parts.Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                foreach (var varible in result)
                {
                    var name = varible.Trim().ToLower();
                    //Debug.WriteLine(name);
                    names.Add(name);
                }
                //Debug.WriteLine($"ITEMS : {string.Join('|', result)}");
            }
            //Debug.WriteLine("");

            return names;
        }
    }
}
