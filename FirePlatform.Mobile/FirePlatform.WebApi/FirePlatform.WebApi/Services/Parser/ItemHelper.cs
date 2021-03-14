using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FirePlatform.WebApi.Model;
using FirePlatform.WebApi.Model.Template;

namespace FirePlatform.WebApi.Services.Parser
{

    public static class ItemHelper
    {
        public static Item Prepare(string Text_line, 
                                   int numID, 
                                   int groupNum, 
                                   string groupTitle, 
                                   string groupTag, 
                                   Dictionary<string, List<ComboItem>> Databases, 
                                   Dictionary<string, string[,]> Matrixes, 
                                   Dictionary<string, string> Memoses, 
                                   Dictionary<string, string> Pictures)
        {
            Item item = new Item();

            Text_line = Text_line.Trim();

            item.TooltipText = Text_line;
            item.GroupID = groupNum;
            item.GroupTitle = groupTitle;
            item.NumID = numID;

            var bracket = Text_line.IndexOf("[", StringComparison.Ordinal);
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
                            //item.VisConditionNameVaribles.AddRange(CalculationTools.GetVaribliNames(item.VisCondition));
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "G:")))
                        {
                            var regex = nt.Replace("G:", "");
                            var indexOfEquals = regex.IndexOf("=", StringComparison.Ordinal);
                            var name = regex.Substring(0, indexOfEquals);
                            var condition = regex.Substring(indexOfEquals + 1);

                            var ghostFormula = new GhostFormula()
                            {
                                Name = name,
                                Tag = nt.Substring(2).Trim().ToLowerInvariant(),
                                Conditions = condition
                            };
                            item.GhostFormulas.Add(ghostFormula);
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "H:")))
                        {
                            item.Type = ItemType.Hidden.ToString();
                            var regex = nt.Replace("H:", "");
                            var indexOfEquals = regex.IndexOf("=", StringComparison.Ordinal);
                            var name = regex.Substring(0, indexOfEquals);
                            var condition = regex.Substring(indexOfEquals + 1);
                            /*var ghostFormula = new GhostFormula()
                            {
                                Name = name,
                                Tag = nt.Substring(2).Trim().ToLowerInvariant(),
                                Conditions = condition
                            };
                            item.GhostFormulas.Add(ghostFormula);*/
                            item.NameVarible = name;
                            item.Formula = condition;
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "N:")))
                        {
                            item.Type = ItemType.Num.ToString();

                            var numstrings = Parser.StringSplit(nt.Substring(2), "#");
                            var numvar = numstrings[0].Trim().ToLowerInvariant();
                            var numvals = Parser.StringSplit(numstrings[1].Trim(), " ");

                            item.Dec = "F" + Convert.ToInt32(numvals[0], System.Globalization.CultureInfo.InvariantCulture);
                            item.Min = Convert.ToDouble(numvals[1], System.Globalization.CultureInfo.InvariantCulture);
                            item.Value = Convert.ToDouble(numvals[2], System.Globalization.CultureInfo.InvariantCulture);
                            item.Max = Convert.ToDouble(numvals[3], System.Globalization.CultureInfo.InvariantCulture);
                            item.Inc = Convert.ToDouble(numvals[4], System.Globalization.CultureInfo.InvariantCulture);
                            item.NameVarible = numvar;
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "C:")))
                        {
                            item.Type = ItemType.Check.ToString();
                            item.Value = false;
                            item.NameVarible = nt.Replace("C:", "").Trim().ToLower();
                        }
                        else if (nt.Length > 2 && nt.Substring(0, 2) == "F:")
                        {
                            item.Type = ItemType.Formula.ToString();

                            if (nt.IndexOf("=") != -1)
                            {
                                var firstPart = nt.Substring(0, nt.IndexOf('=') + 1);
                                item.NameVarible = firstPart.Replace("F:", "").Replace("=", "").Trim().ToLower();
                                item.Formula = nt.Replace(firstPart, "").Trim().ToLowerInvariant();
                            }

                            var regex = new Regex(@"(\w*mx\w*)|'(\w+)+'");
                            var matches = regex.Matches(item.Formula);
                            if (matches != null && matches.Count == 2)
                            {
                                var matrixName = matches[1].Value.Trim('\'');
                                if (!string.IsNullOrWhiteSpace(matrixName) && Matrixes != null && Matrixes.ContainsKey(matrixName))
                                {
                                    item.Matrix = Matrixes[matrixName];
                                }
                            }
                        }
                        else if (nt.Length > 2 && nt.Substring(0, 2) == "B:")
                        {
                            item.Type = ItemType.BackCalc.ToString();
                            item.NameVarible = nt.Replace("B:", "").Split('=')[0].Trim().ToLower();
                            item.Formula = nt.Replace("B:", "").Trim().Substring(item.NameVarible.Length + 1).Trim().ToLowerInvariant();
                            if (item.Formula.Contains("#"))
                            {
                                item.Formula = item.Formula.Split('#')[0];
                            }
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "D:")))
                        {
                            item.Type = ItemType.Combo.ToString();

                            var nameSelectedItem = new List<string>();

                            var valtag = nt.Substring(2).Trim().ToLowerInvariant();

                            var selectedDb = Databases[valtag];

                            item.ComboItems.AddRange(selectedDb);
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "L:")))
                        {
                            item.Type = ItemType.Combo.ToString();

                            var tag = nodetags[0];
                            var listitems = Parser.StringSplit(tag.Substring(2), ",");
                            if (listitems.Length % 2 != 0)
                            {
                                item.NameVaribleMatrix = listitems[0];
                                var list = listitems.ToList();
                                list.RemoveAt(0);
                                listitems = list.ToArray();
                            }

                            for (int index = 0; index < listitems.Length - 1; index += 2)
                            {
                                var displayName = listitems[index].Trim();
                                if (item.Title.ToLower().Trim() == displayName.Trim().ToLower()) continue;

                                var groupKey = listitems[index + 1].Trim().ToLowerInvariant();
                                var comboItem = new ComboItem()
                                {
                                    DisplayName = displayName,
                                    GroupKey = groupKey,
                                    IsVisible = true
                                };

                                item.ComboItems.Add(comboItem);
                            }

                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "R:")))
                        {
                            //valtag = nt.Substring(2).Trim().ToLowerInvariant();
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "T:")))
                        {
                            if (nt.Contains("="))
                                item.NameVarible = nt.Replace("T:", "").Split('=')[0].Trim().ToLower();
                            item.Type = ItemType.Text.ToString();
                            item.Value = nt.Substring(2);
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "M:")))
                        {
                            if (nt.Contains("="))
                                item.NameVarible = nt.Replace("M:", "").Split('=')[0].Trim().ToLower();
                            item.Type = ItemType.Text.ToString();
                            var name = nt.Substring(2).Trim();
                            if (Memoses.ContainsKey(name))
                                item.Value = Memoses[name];
                            else
                                item.Value = "MEMOS NOT FOUND";
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "P:")))
                        {
                            var pictureName = nt.Replace("P:", "").Split('=')[0].Trim().ToLower();
                            if (Pictures.ContainsKey(pictureName))
                            {
                                item.Picture = new Picture()
                                {
                                    Name = pictureName,
                                    Data = Pictures[pictureName]
                                };
                            }
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "W:")))
                        {
                            item.Type = ItemType.Fulltext.ToString();
                            item.NameVarible = nt.Substring(2).Trim().ToLowerInvariant();
                        }
                        item.InitialValue = item.Value;
                        item.NameVarible = item.NameVarible.Trim();
                        item.IsVisible = string.IsNullOrEmpty(item.VisCondition);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            if (string.IsNullOrWhiteSpace(item.Type))
            {
                item.Type = ItemType.HTML.ToString();
                var parts = Text_line.Split("\t");
                var visibility = parts[0];
                if (!"-".Equals(visibility))
                {
                    item.VisCondition = visibility;
                }
                string text = string.Empty;
                for (int i = 1; i < parts.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(parts[i]))
                    {
                        if (item.HtmlLevel == -1)
                        {
                            item.HtmlLevel = i;
                        }
                        text += parts[i];
                    }
                }

                item.Value = text ?? string.Empty;
            }
            return item;
        }

        public static string RemoveHeaderFromHtmlElements(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var splittedText = text.Split("\t");

            return splittedText[splittedText.Length - 1];
        }
    }
}


