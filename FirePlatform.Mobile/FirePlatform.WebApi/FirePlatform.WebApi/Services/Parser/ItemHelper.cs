using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FirePlatform.WebApi.Model;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services.Tools;
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
        public static _Item Prepare(string Text_line, int numID, int groupNum, string groupTitle, string groupTag, Dictionary<string, List<ComboItem>> Databases)
        {
            _Item item = new _Item();

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
                            var parts = nt.Replace("G:", "").Split("=");
                            var ghostFormula = new GhostFormula()
                            {
                                Name = parts[0],
                                Conditions = parts[1],
                                Tag = nt.Substring(2).Trim().ToLowerInvariant()
                            };
                            item.GhostFormulas.Add(ghostFormula);
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "H:")))
                        {
                            var parts = nt.Replace("H:", "").Split("=");
                            var ghostFormula = new GhostFormula()
                            {
                                Name = parts[0],
                                Conditions = parts[1],
                                Tag = nt.Substring(2).Trim().ToLowerInvariant()
                            };
                            item.GhostFormulas.Add(ghostFormula);
                        }
                        else if (((nt.Length > 2) && (nt.Substring(0, 2) == "N:")))
                        {
                            item.Type = ItemType.Num.ToString();

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
                            item.Value = false;
                            item.NameVarible = nt.Replace("C:", "").Trim().ToLower();
                        }
                        else if (nt.Length > 2 && nt.Substring(0, 2) == "F:")
                        {
                            item.Type = ItemType.Formula.ToString();
                            item.NameVarible = nt.Replace("F:", "").Split('=')[0].Trim().ToLower();
                            item.Formula = nt.Substring(2).Trim().ToLowerInvariant();
                        }
                        else if (nt.Length > 2 && nt.Substring(0, 2) == "B:")
                        {
                            item.Type = ItemType.BackCalc.ToString();
                            item.NameVarible = nt.Replace("B:", "").Split('=')[0].Trim().ToLower();
                            item.Formula = nt.Substring(2).Trim().ToLowerInvariant();
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


                            for (int index = 0; index < listitems.Length - 1; index += 2)
                            {
                                var displayName = listitems[index].Trim();
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
                            item.Type = ItemType.Text.ToString();
                            item.Value = nt.Substring(2);
                        }
                        item.InitialValue = item.Value;
                        item.NameVarible = item.NameVarible.Trim();
                        item.IsVisible = string.IsNullOrEmpty(item.VisCondition);
                    }
                }
                catch (Exception ex)
                {

                }

            }
            return item;
        }
    }
}


