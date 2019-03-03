using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NCalc;

namespace FirePlatform.WebApi.Services.Tools
{
    public static class CalculationTools
    {
        public static List<string> NotFoundName = new List<string>();
        public static double Calculate(string formula, Dictionary<string, object> parameters)
        {
            try
            {
                Expression expression = new Expression(formula, EvaluateOptions.IgnoreCase)
                {
                    Parameters = parameters
                };
                string result = Convert.ToDouble(expression.Evaluate()).ToString();
                if (double.TryParse(result, out double res))
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FORMULAS] {formula} - {ex.Message} \n");
            }
            return default(double);
        }

        public static bool? CalculateVis(string formula, Dictionary<string, object> parameters)
        {
            try
            {
                Expression expression = new Expression(formula.ToLower(), EvaluateOptions.IgnoreCase)
                {
                    Parameters = parameters
                };
                string result = Convert.ToBoolean(expression.Evaluate()).ToString();
                if (bool.TryParse(result, out bool res))
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                if (!NotFoundName.Contains(ex.Message))
                {
                    NotFoundName.Add(ex.Message);
                }
                //System.Diagnostics.Debug.WriteLine($"[VISIBILITY] {formula} - {ex.Message} \n");
                return null;
            }
            return default(bool);
        }

        public static List<string> GetVaribliNames(string condition)
        {
            var names = new List<string>();

            if (string.IsNullOrEmpty(condition)) return names;
            //Debug.WriteLine($"ITEM VISIBILITY : {condition}");

            char[] splitChars = new char[] { '&', '|', '!', '(', ')', '=', '<', '>' };
            var parts = condition.Split(splitChars).ToList();
            if (parts.Any())
            {
                var result = parts.Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();
                foreach (var varible in result)
                {
                    var name = varible.Trim();
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
