using System;
using FirePlatform.Mobile.Common.Entities;
using System.Linq;
using NCalc;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FirePlatform.Mobile.Common
{
    public static class FormulaHelper
    {
        private static char[] operands = new char[] { '+', '-', '/', '*' };
        public static void PreparingFormula(this ObservableCollection<Item> items)
        {
            var varibleDitionary = items
            .Where(x => !string.IsNullOrEmpty(x.NumVar))
            .Select(x => new { key = x.NumVar, value = x.NumValue })
            .ToDictionary(x => x.key, y => (object)y.value);

            foreach (var item in items)
            {
                if (item.Type.ToLower() == ControlType.formula.ToString())
                {
                    if (item.Formula.Contains("="))
                    {
                        var name = string.Empty;
                        var formula = GetFormulaString(item.Formula.Replace("(","").Replace(")",""), out name);
                        item.NumVar = name;
                        item.NumValueString = Calculate(formula, varibleDitionary).ToString();
                        if (varibleDitionary.ContainsKey(item.NumVar))
                        {
                            varibleDitionary[item.NumVar] = item.NumValue;
                        }
                        else
                        {
                            varibleDitionary.Add(item.NumVar, item.NumValue);
                        }
                    }
                    else
                    {
                        var name = item.Formula.Split(' ')[0].Trim();
                        if (varibleDitionary.ContainsKey(name))
                        {
                            //item.NumValue = varibleDitionary[name];
                        }
                    }

                }
                else if (item.Type.ToLower() == ControlType.combo.ToString())
                {
                    var filter = item.MultiItemTags.Select(x => x.Split(',')[0]).ToArray().Distinct();
                    if (filter.Count() == 1)
                    {
                        item.NumVar = filter.FirstOrDefault();
                        if (varibleDitionary.ContainsKey(item.NumVar))
                        {
                            if (int.TryParse(item.SelectedIndex, out int index) && index > -1)
                            {
                                item.NumValueString = item.MultiItemTags[index].Split(',')[1];
                            }
                            varibleDitionary[item.NumVar] = item.NumValueString;
                        }
                        else
                        {
                            if (int.TryParse(item.SelectedIndex, out int index) && index > -1)
                            {
                                item.NumValueString = item.MultiItemTags[index].Split(',')[1];

                            }
                            varibleDitionary.Add(item.NumVar, item.NumValueString);
                        }
                    }
                }
            }
        }

        private static double Calculate(string formula, Dictionary<string, object> parameters)
        {
            try
            {
                Expression expression = new Expression(formula, EvaluateOptions.IgnoreCase);

                expression.Parameters = parameters;
                string result = Convert.ToDouble(expression.Evaluate()).ToString();
                if (double.TryParse(result, out double res))
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{formula} - {ex.Message} \n");
            }
            return default(double);
        }

        private static string GetFormulaString(string fullFormula, out string nameVarible)
        {
            var parts = fullFormula.Split('=');
            nameVarible = parts[0].Trim().ToLower();
            return parts[1].Trim().Split(' ')[0].Trim();
        }
    }
}
