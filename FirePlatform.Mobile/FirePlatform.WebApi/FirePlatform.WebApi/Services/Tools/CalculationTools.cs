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

        public static object CalculateFormulasMatrix(string expr, string[,] matrix, Dictionary<string, object> parameters)
        {
            try
            {
                object result = "ERROR";
                NCalc.Expression expr1 = new NCalc.Expression(expr, EvaluateOptions.IgnoreCase);
                string param1 = null;
                string param2 = null;
                foreach (var keyValue in parameters)
                {
                    if (param1 == null && keyValue.Value != null)
                    {
                        param1 = keyValue.Value.ToString();
                    }
                    else if (param2 == null && keyValue.Value != null)
                    {
                        param2 = keyValue.Value.ToString();
                    }
                    expr1.Parameters[keyValue.Key] = keyValue.Value;
                }
                if (expr1.Parameters.Count == 2)
                {
                    if (!string.IsNullOrWhiteSpace(param1) && !string.IsNullOrWhiteSpace(param2))
                        result = GetDataFromMatrix(matrix, param1, param2);
                }
                else if (expr1.Parameters.Count == 1)
                {
                    if (!string.IsNullOrWhiteSpace(param1))
                        result = GetDataFromMatrix(matrix, param1);
                }
                return result;
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
        private static object GetDataFromMatrix(string[,] matrix, string Ystring, string Xstring = null)
        {
            bool Yonly = false;
            string xstring = null;
            if ((Xstring == null))
            {
                Yonly = true;
            }
            else
            {
                xstring = Convert.ToString(Xstring);
            }

            string ystring = Convert.ToString(Ystring);
            bool Xcond = false;
            bool Ycond = false;
            double xval;
            double yval;
            int xsize;
            int ysize;
            string result = null;
            object expresult = null;
            int xx = 0;
            int yy = 0;
            string[] ydata = null;
            string[] xdata = null;
            NCalc.Expression exp;
            ysize = matrix.GetUpperBound(1);
            xsize = matrix.GetUpperBound(0);

            xdata = new string[xsize];
            ydata = new string[ysize];

            if (!Yonly)
            {
                for (int t = 1; t <= xsize; t++)
                {
                    xdata[t - 1] = matrix[t, 0].ToLowerInvariant();
                    if ((t == 1))
                    {
                        exp = new NCalc.Expression(xdata[t - 1]);
                        exp.Parameters["x"] = 1;
                        expresult = null;
                        try { expresult = exp.Evaluate(); } catch { }

                        if ((expresult != null && expresult.GetType() == typeof(bool)))
                        {
                            Xcond = true;
                        }
                    }
                }
            }

            for (int t = 1; t <= ysize; t++)
            {
                ydata[t - 1] = matrix[0, t].ToLowerInvariant();
                if ((t == 1))
                {

                    exp = new NCalc.Expression(ydata[(t - 1)]);
                    exp.Parameters["y"] = 1;
                    expresult = null;
                    try { expresult = exp.Evaluate(); } catch { }
                    if ((expresult != null && expresult.GetType() == typeof(bool)))
                    {
                        Ycond = true;
                    }
                }

            }

            if (Yonly) { xx = 0; }
            else if (IsNumeric(xdata[1]))
            {
                double.TryParse(xstring, out xval);
                xx = xdata.ToList().IndexOf(xdata.OrderBy(x => Math.Abs(Convert.ToDouble(x) - xval)).First());
            }
            else if (Xcond)
            {
                foreach (string datax in xdata)
                {
                    exp = new NCalc.Expression(datax);
                    exp.Parameters["x"] = double.Parse(xstring);
                    if ((Convert.ToBoolean(exp.Evaluate()) == true))
                    {
                        xx = xdata.ToList().IndexOf(datax);
                        break;
                    }
                }
            }
            else
            {
                xx = xdata.ToList().IndexOf(xstring.ToLowerInvariant());
            }

            if (IsNumeric(ydata[1]))
            {
                yval = double.Parse(ystring);
                yy = ydata.ToList().IndexOf(ydata.OrderBy(x => Math.Abs(Convert.ToDouble(x) - yval)).First());
            }
            else
            {
                NCalc.Expression le = new NCalc.Expression(ydata[1]);
                object res = null;
                if (Ycond)
                {
                    foreach (string datay in ydata)
                    {
                        le = new NCalc.Expression(datay);
                        res = null;
                        le.Parameters["y"] = double.Parse(ystring);
                        if ((Convert.ToBoolean(le.Evaluate()) == true))
                        {
                            yy = ydata.ToList().IndexOf(datay);
                            break;
                        }
                    }
                }
                else
                {
                    yy = ydata.ToList().IndexOf(ystring.ToLowerInvariant());
                }
            }

            try
            {
                result = matrix[(xx + 1), (yy + 1)];
            }
            catch (Exception ex)
            {
            }

            return result;

        }
        private static bool IsNumeric(string s)
        {
            bool result = false;
            bool charcheck = true;
            double dbl;

            foreach (char st in s) { if (!st.ToString().All(char.IsDigit) || !st.ToString().All(char.IsPunctuation)) { charcheck = false; } }

            if (charcheck)
            {
                double.TryParse(s, out dbl);
                result = !double.IsNaN(dbl);
            }

            return result;
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
