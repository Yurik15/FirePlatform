using System;
using System.Collections.Generic;
using NCalc;

namespace FirePlatform.WebApi.Services.Tools
{
    public static class CalculationTools
    {
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
                Expression expression = new Expression(formula, EvaluateOptions.IgnoreCase)
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
                System.Diagnostics.Debug.WriteLine($"[VISIBILITY] {formula} - {ex.Message} \n");
                return null;
            }
            return default(bool);
        }
    }
}
