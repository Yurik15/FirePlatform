using System;
using System.Collections.Generic;
using System.Linq;
using FirePlatform.WebApi.Model;
using NCalc;

namespace FirePlatform.WebApi.Services.Parser
{

    public static class ItemHelper
    {
        public static _Item PreparePicture(string Text_line, List<string> dataPictures)
        {
            _Item item = new _Item();
            item.Title = Text_line.Split(' ')[1];
            item.Value = string.Join(" ", dataPictures);
            item.Type = ItemType.Picture.ToString();
            return item;
        }
        public static _Item Prepare(string Text_line, int numID, int groupNum, string groupTitle, string groupTag, Dictionary<String, List<String>> Databases)
        {
            _Item item = new _Item();

            Text_line = Text_line.Trim();

            item.TooltipText = Text_line;
            item.GroupID = groupNum;
            item.GroupTitle = groupTitle;
            item.NumID = numID;

            var bracket = Text_line.IndexOf("[");
            if (bracket > -1)
            {
                try
                {
                    item.Title = Text_line.Remove(bracket).Trim();

                    var nodetag = Text_line.Substring(bracket);
                    var nodetags = Parser.GetTags(nodetag).ToArray();

                    foreach (String nt in nodetags)
                    {
                        if (((nt.Length > 2) && (nt.Substring(0, 2) == "V:")))
                        {
                            item.VisCondition = nt.Substring(2).Trim().ToLowerInvariant();
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "G:")))
                        {
                            var parts = nt.Replace("G:", "").Split("=");
                            item.NameVarible = parts[0];
                            item.Value = parts[1];
                            item.GhostFormulas.Add(nt.Substring(2).Trim().ToLowerInvariant());
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "H:")))
                        {
                            item.NameVarible = nt.Replace("H:", "").Split("=")[0];
                            item.Type = ItemType.Hidden.ToString();
                            item.GhostFormulas.Add(nt.Substring(2).Trim().ToLowerInvariant());
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "N:")))
                        {
                            item.Type = ItemType.Num.ToString();
                            item.IsVisible = true;

                            var numstrings = Parser.StringSplit(nt.Substring(2), "#");
                            var numvar = numstrings[0].Trim().ToLowerInvariant();
                            var numvals = Parser.StringSplit(numstrings[1].Trim(), " ");

                            item.Dec = "F" + Convert.ToInt32(numvals[0]);
                            item.Min = Convert.ToDouble(numvals[1]);
                            item.Value = Convert.ToDouble(numvals[2]);
                            item.Max = Convert.ToDouble(numvals[3]);
                            item.Inc = Convert.ToDouble(numvals[4]);
                            item.NameVarible = numvar;
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "C:")))
                        {
                            item.Type = ItemType.Check.ToString();
                            item.IsVisible = true;
                            item.Value = false;
                            item.NameVarible = nt.Replace("C:", "").Trim().ToLower();
                        }
                        else if (nt.Length > 2 && nt.Substring(0, 2) == "F:")
                        {
                            item.Type = ItemType.Formula.ToString();
                            item.IsVisible = true;
                            item.NameVarible = nt.Replace("F:", "").Split('=')[0].Trim().ToLower();
                            item.Formula = nt.Substring(2).Trim().ToLowerInvariant();
                        }
                        else if (nt.Length > 2 && nt.Substring(0, 2) == "B:")
                        {
                            item.Type = ItemType.BackCalc.ToString();
                            item.IsVisible = true;

                            item.Formula = nt.Substring(2).Trim().ToLowerInvariant();
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "D:")))
                        {
                            item.Type = ItemType.Combo.ToString();
                            item.IsVisible = true;

                            var nameSelectedItem = new List<string>();

                            var valtag = nt.Substring(2).Trim().ToLowerInvariant();
                            List<string> tempstrlist1 = new List<string>();
                            List<string> tempstrlist2 = new List<string>();
                            List<MyComboItem> tempDict = new List<MyComboItem>();

                            for (int t = 0; t < Databases[valtag].Count(); t++)
                            {
                                var tempstr = Databases[valtag][t];
                                tempstrlist1.Add(tempstr.Substring(0, tempstr.IndexOf(",")));
                                tempstrlist2.Add(tempstr.Substring(1 + tempstr.IndexOf(",")));
                            }
                            item.MultiItemTitles = tempstrlist1;
                            item.MultiItemTags = tempstrlist2;
                            item.NameVarible = string.Join("|", nameSelectedItem);

                            for (int t = 0; t < item.MultiItemTitles.Count(); t++)
                            {
                                var comboItem = new MyComboItem();
                                comboItem.ComboItemTitle = tempstrlist1[t];
                                comboItem.ComboItemTag = tempstrlist2[t];
                                tempDict.Add(comboItem);
                            }
                            item.MultiItemDict = tempDict;
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "L:")))
                        {
                            item.Type = ItemType.Combo.ToString();
                            item.IsVisible = true;

                            var nameSelectedItem = new List<string>();

                            List<string> tempstrlist1 = new List<string>();
                            List<string> tempstrlist2 = new List<string>();
                            List<MyComboItem> tempDict = new List<MyComboItem>();

                            var varname = "";

                            var nt0 = nodetags[0];
                            var listitems = Parser.StringSplit(nt0.Substring(2), ",");
                            var listitemslist = listitems.ToList();
                            var c = listitems.Count();

                            int rmd;
                            Math.DivRem(c, 2, out rmd);
                            bool varOption = false;
                            if (rmd == 1)
                            {
                                varname = listitems[0].Trim().TrimEnd("\t".ToCharArray());
                                listitemslist.RemoveAt(0);
                                varOption = true;
                                c -= 1;
                            }
                            for (int u = 0; u < c - 1; u += 2)
                            {
                                tempstrlist1.Add(listitemslist[u].Trim());

                                if (varOption)
                                {
                                    nameSelectedItem.Add(listitemslist[u + 1].Trim().ToLowerInvariant());
                                    tempstrlist2.Add(varname + "," + listitemslist[u + 1].Trim().ToLowerInvariant() + ",");
                                }
                                else
                                {
                                    string tmpstrall = listitemslist[u + 1].Trim().ToLowerInvariant();
                                    string[] tmpstrsplit = Parser.StringSplit(tmpstrall, "|");
                                    string tmpline = "";

                                    foreach (string tmpstr in tmpstrsplit)
                                    {
                                        nameSelectedItem.Add(tmpstr);
                                        if (!tmpstr.StartsWith("-"))
                                        {
                                            tmpline += tmpstr + ",true,";
                                        }
                                        else
                                        {
                                            tmpline += tmpstr.Substring(1) + ",false,";
                                        }
                                    }
                                    if (tmpline != "") tmpline.TrimEnd(Convert.ToChar(","));
                                    tempstrlist2.Add(tmpline);

                                }
                            }
                            item.MultiItemTitles = tempstrlist1;
                            item.MultiItemTags = tempstrlist2;
                            item.Varibles = string.Join("|", nameSelectedItem);

                            for (int t = 0; t < item.MultiItemTitles.Count(); t++)
                            {
                                var comboItem = new MyComboItem();
                                comboItem.ComboItemTitle = tempstrlist1[t];
                                comboItem.ComboItemTag = tempstrlist2[t];
                                tempDict.Add(comboItem);
                            }

                            item.MultiItemDict = tempDict;
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "R:")))
                        {
                            //valtag = nt.Substring(2).Trim().ToLowerInvariant();
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "T:")))
                        {
                            item.Type = ItemType.Text.ToString();
                            item.Value = nt.Substring(2);
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            }
            return item;
        }

        public static void NotifyAboutChange(this _Item item)
        {
            foreach (var relatedGroup in item.RelatedGroups)
            {
                relatedGroup.UpdateGroup();
            }
            foreach (var relatedItem in item.RelatedItems)
            {
                relatedItem.UpdateItem();
            }
        }
        public static void UpdateItem(this _Item item)
        {
            var paramsDic = item.GetParams();
            var formula = item.GetFormulaString();
            if (!string.IsNullOrEmpty(formula))
            {
                var resultValue = Calculate(formula, paramsDic);
                item.Value = resultValue;
            }
            if (!string.IsNullOrEmpty(item.VisCondition))
            {
                var resultValue = CalculateVis(item.VisCondition, paramsDic);
                item.IsVisible = resultValue;
            }

        }
        public static void UpdateGroup(this _ItemGroup itemGroup)
        {
            var paramsDic = itemGroup.GetParams();
        }
        public static Dictionary<string, object> GetParams(this _Item item)
        {
            var paramsDic = new Dictionary<string, object>();
            foreach (var relatedItem in item.RelatedItems)
            {
                if (!paramsDic.ContainsKey(relatedItem.NameVarible))
                {
                    paramsDic.Add(relatedItem.NameVarible, relatedItem.Value);
                }
            }
            return paramsDic;
        }
        public static Dictionary<string, object> GetParams(this _ItemGroup item)
        {
            var paramsDic = new Dictionary<string, object>();
            foreach (var relatedItem in item.RelatedItem)
            {
                if (!paramsDic.ContainsKey(relatedItem.NameVarible))
                {
                    paramsDic.Add(relatedItem.NameVarible, relatedItem.Value);
                }
            }
            return paramsDic;
        }
        public static double Calculate(string formula, Dictionary<string, object> parameters)
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

        public static bool CalculateVis(string formula, Dictionary<string, object> parameters)
        {
            try
            {
                Expression expression = new Expression(formula, EvaluateOptions.IgnoreCase);

                expression.Parameters = parameters;
                string result = Convert.ToBoolean(expression.Evaluate()).ToString();
                if (bool.TryParse(result, out bool res))
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{formula} - {ex.Message} \n");
            }
            return default(bool);
        }
    }
}


