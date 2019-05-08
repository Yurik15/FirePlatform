using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FirePlatform.WebApi.Model;
using FirePlatform.WebApi.Model.Template;
using FirePlatform.WebApi.Services.Tools;

namespace FirePlatform.WebApi.Services.Parser
{
    public static class Parser
    {
        public static List<ItemGroup> PrepareControls(string fileContent)
        {
            var startDate = DateTime.Now;
            var controls = ParseDoc(fileContent);
            var endDate = DateTime.Now;
            var result = endDate - startDate;
            Debug.WriteLine($"[ParseDoc] - Time - minutes : {result.Minutes} or seconds : {result.Seconds}");

            startDate = DateTime.Now;
            var varibles = PrepareFieldNameDependToItem(controls);
            endDate = DateTime.Now;
            result = endDate - startDate;
            Debug.WriteLine($"[ParseDoc] - Time - minutes : {result.Minutes} or seconds : {result.Seconds}");

            startDate = DateTime.Now;
            PrepareGroupDependToItems(controls, varibles);
            endDate = DateTime.Now;
            result = endDate - startDate;
            Debug.WriteLine($"[ParseDoc] - Time - minutes : {result.Minutes} or seconds : {result.Seconds}");

            return controls;
        }

        #region parser
        private static List<ItemGroup> ParseDoc(string fileContent)
        {
            try
            {
                string DATABASE = "Database";
                string SPACE = " ";
                string COMMENT_LINE = "---";
                string END = "END";
                string COMMA_SEPARATOR = ",";
                char T = '\t';
                char NEW_LINE = '\n';

                var result = new List<ItemGroup>();
                Dictionary<string, List<ComboItem>> Databases = new Dictionary<string, List<ComboItem>>();
                List<(string title, string part1, string part2, string full)> ComposeComboItem = new List<(string title, string part1, string part2, string full)>();

                string[] initLines = fileContent.Split(NEW_LINE);


                Dictionary<Tuple<string, string>, List<string>> data = new Dictionary<Tuple<string, string>, List<string>>();
                foreach (var line in initLines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith(COMMENT_LINE, StringComparison.Ordinal)) continue;

                    if (line.StartsWith(SPACE, StringComparison.Ordinal) || line.StartsWith(T.ToString(), StringComparison.Ordinal))
                    {
                        data.Last().Value.Add(line.Trim());
                    }
                    else
                    {
                        string tag = string.Empty;
                        string title = line.Trim();

                        var bracket = line.IndexOf("[", StringComparison.Ordinal);
                        if (bracket > -1)
                        {
                            title = line.Substring(0, bracket).Trim();
                            tag = string.Join(COMMA_SEPARATOR, GetTags(line));
                        }
                        data.Add(new Tuple<string, string>(title, tag), new List<string>());
                    }
                }

                foreach (var group in data)
                {
                    var title = group.Key.Item1;
                    var tag = group.Key.Item2;
                    var itemsFromGroup = group.Value;

                    if (title.StartsWith(DATABASE, StringComparison.Ordinal))
                    {
                        var tempDB = new List<ComboItem>();
                        var dbTitle = title.Substring(DATABASE.Length).Trim().TrimStart(T).ToLowerInvariant();

                        foreach (var itemText in itemsFromGroup)
                        {
                            var line = itemText.Trim().TrimStart(T);
                            var dbItems = StringSplit(line, line.IndexOf(",\t") == -1 ? ", \t" : ",\t"); //COMMA_SEPARATOR);

                            var displayName = dbItems[0];
                            var groupKey = String.Join(",", dbItems.Skip(1)).Trim(',');
                            if (groupKey.Contains(","))
                            {
                                var parts = groupKey.Split(",");
                                ComposeComboItem.Add((dbTitle, parts[0], parts[1], groupKey));
                            }
                            var comboItem = new ComboItem()
                            {
                                DisplayName = displayName,
                                GroupKey = groupKey
                            };
                            tempDB.Add(comboItem);
                        }
                        Databases.Add(dbTitle, tempDB);
                    }
                }

                int indexGroup = 0;
                foreach (var group in data)
                {
                    var title = group.Key.Item1;
                    var tag = group.Key.Item2;
                    var itemsFromGroup = group.Value;

                    if (title.StartsWith(DATABASE, StringComparison.Ordinal)) continue;

                    var items = new List<Item>();

                    if ("picture".Equals(group.Key.Item1.Split(' ')[0].ToLower()))
                    {
                        var item = ItemHelper.PreparePicture(group.Key.Item1, itemsFromGroup);
                        //result.Add(new ItemGroup(indexGroup, title, tag, item));
                    }
                    else
                    {
                        var groupItems = new ItemGroup
                        {
                            IndexGroup = indexGroup,
                            Title = title,
                            Tag = tag,
                            IsVisible = true,
                            InitialVisibility = true
                        };

                        for (int indexItems = 0; indexItems < itemsFromGroup.Count; indexItems++)
                        {
                            var itemText = itemsFromGroup[indexItems];
                            if (itemText.StartsWith(END, StringComparison.Ordinal)) break;
                            else if (string.IsNullOrWhiteSpace(itemText) || itemText.StartsWith(COMMENT_LINE, StringComparison.Ordinal)) continue;

                            var item = ItemHelper.Prepare(itemText, indexItems, indexGroup, title, tag, Databases);
                            item.ParentGroup = groupItems;
                            //if (!string.IsNullOrEmpty(item.Type))
                            items.Add(item);
                        }

                        groupItems.Items = items;

                        if (!string.IsNullOrEmpty(tag))
                        {
                            groupItems.VisCondition = tag.Split(':')[1].Trim();
                            //groupItems.VisConditionNameVaribles.AddRange(CalculationTools.GetVaribliNames(groupItems.VisCondition));
                            groupItems.IsVisible = false;
                            groupItems.InitialVisibility = false;
                        }


                        result.Add(groupItems);
                        indexGroup++;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        #endregion parser

        #region prepare depend controls
        private static void PrepareGroupDependToItems(List<ItemGroup> _ItemGroups, List<DataDependItem> data)
        {

            //-------DON'T REMOVE
            /*foreach (var a in data)
            {
                if (a.Name.Contains("|") || a.Name.Contains(",") || a.Name.Contains("."))
                {
                    Debug.WriteLine($"TYPE : {a.ReferencedItem.Type}, GROUP_ID : {a.ReferencedItem.GroupID}, NUM_ID : {a.ReferencedItem.NumID}, VALUE : {a.Name}");
                }
            }*/


            foreach (var group in _ItemGroups)
            {
                if (!string.IsNullOrEmpty(group.VisCondition))
                {
                    var relatedItems = new List<DataDependItem>();
                    foreach (var varibleName in group.VisConditionNameVaribles)
                    {
                        foreach (var dependElement in data)
                        {
                            var needToAdd = false;
                            var nameDependElement = dependElement.Name;
                            if (nameDependElement.Contains(","))
                            {
                                var parts = nameDependElement.Split(",").Select(y => y.Trim().ToLower());
                                needToAdd = parts.Contains(varibleName.Trim().ToLower());
                            }
                            else
                            {
                                needToAdd = nameDependElement.Trim().ToLower() == varibleName.Trim().ToLower();
                            }
                            if (needToAdd)
                                relatedItems.Add(dependElement);
                        }
                    }
                    group.DependToItems = relatedItems;
                    relatedItems.ForEach(x => x.ReferencedItem.NeedNotifyGroups.Add(group));

                    var paramsDic = ItemExtentions.GetParams(group.DependToItems);
                    var res = CalculationTools.CalculateVis(group.VisCondition, paramsDic);
                    group.IsVisible = res.HasValue ? res.Value : false;
                }
                foreach (var item in group.Items)
                {
                    bool isVisibleConditions = false;
                    string condition = item.VisCondition ?? string.Empty;
                    if (item.Type == ItemType.Formula.ToString())
                    {
                        if (string.IsNullOrWhiteSpace(item.Formula))
                        {
                            throw new Exception("the item with 'FORMULA' type can't be null or empty!");
                        }
                        var formula = item.Formula;
                        condition += " " + formula;
                    }
                    if (!string.IsNullOrEmpty(condition))
                    {
                        isVisibleConditions = true;
                        var relatedItems = new List<DataDependItem>();
                        foreach (var varibleName in item.VisConditionNameVaribles)
                        {
                            var elmentesBeforCurrentElement = data.Where(x => x.ReferencedItem.NumID < item.NumID && x.ReferencedItem.GroupID == item.GroupID).ToList();
                            if (item.GroupID > 0)
                            {
                                elmentesBeforCurrentElement.AddRange(data.Where(x => x.ReferencedItem.GroupID < item.GroupID).ToList());
                            }
                            if (elmentesBeforCurrentElement.Count == 0)
                            {
                                elmentesBeforCurrentElement = data.Where(x => x.ReferencedItem.NumID < item.NumID || x.ReferencedItem.GroupID < item.GroupID).ToList();
                            }
                            if (elmentesBeforCurrentElement.Count == 0)
                            {
                                throw new Exception("The element doesn't exists before!!!");
                            }

                            foreach (var dependElement in elmentesBeforCurrentElement)
                            {
                                var needToAdd = false;
                                var nameDependElement = dependElement.Name;
                                if (nameDependElement.Contains(","))
                                {
                                    var parts = nameDependElement.Split(",").Select(y => y.Trim().ToLower());
                                    needToAdd = parts.Contains(varibleName.Trim().ToLower());
                                }
                                else
                                {
                                    needToAdd = nameDependElement.Trim().ToLower() == varibleName.Trim().ToLower();
                                }
                                if (needToAdd)
                                    relatedItems.Add(dependElement);
                            }

                        }
                        item.DependToItems = relatedItems;
                        relatedItems.ForEach(x => x.ReferencedItem.NeedNotifyItems.Add(item));

                    }
                    if (!string.IsNullOrEmpty(item.Formula))
                    {
                        var relatedItems = new List<DataDependItem>();
                        foreach (var varibleName in item.FormulaNameVaribles)
                        {
                            var elmentesBeforCurrentElement = data.Where(x => x.ReferencedItem.NumID < item.NumID && x.ReferencedItem.GroupID == item.GroupID).ToList();
                            if (item.GroupID > 0)
                            {
                                elmentesBeforCurrentElement.AddRange(data.Where(x => x.ReferencedItem.GroupID < item.GroupID).ToList());
                            }
                            if (elmentesBeforCurrentElement.Count == 0)
                            {
                                elmentesBeforCurrentElement = data.Where(x => x.ReferencedItem.NumID < item.NumID || x.ReferencedItem.GroupID < item.GroupID).ToList();
                            }
                            if (elmentesBeforCurrentElement.Count == 0)
                            {
                                throw new Exception("The element doesn't exists before!!!");
                            }

                            foreach (var dependElement in elmentesBeforCurrentElement)
                            {
                                var needToAdd = false;
                                var nameDependElement = dependElement.Name;
                                if (nameDependElement.Contains(","))
                                {
                                    var parts = nameDependElement.Split(",").Select(y => y.Trim().ToLower());
                                    needToAdd = parts.Contains(varibleName.Trim().ToLower());
                                }
                                else
                                {
                                    needToAdd = nameDependElement.Trim().ToLower() == varibleName.Trim().ToLower();
                                }
                                if (needToAdd)
                                    relatedItems.Add(dependElement);
                            }

                        }
                        item.DependToItemsForFormulas = relatedItems;
                        relatedItems.ForEach(x => x.ReferencedItem.NeedNotifyItems.Add(item));

                        if (item.ParentGroup.IsVisible) // PERFORMANCE
                        {
                            //--------//
                            var paramsDic = ItemExtentions.GetParams(item.DependToItemsForFormulas);
                            var res = CalculationTools.CalculateFormulas(item.Formula, paramsDic);
                            item.Value = res;
                            //--------//
                        }
                    }
                    if (isVisibleConditions)
                    {
                        if (item.ParentGroup.IsVisible) // PERFORMANCE
                        {
                            var paramsDic = ItemExtentions.GetParams(item.DependToItems);
                            var res = CalculationTools.CalculateVis(item.VisCondition, paramsDic);
                            item.IsVisible = res.HasValue ? res.Value : false;
                        }
                    }
                }
            }
        }

        private static List<DataDependItem> PrepareFieldNameDependToItem(List<ItemGroup> _ItemGroups)
        {
            List<DataDependItem> dataDependItem = new List<DataDependItem>();
            foreach (var group in _ItemGroups)
            {
                foreach (var item in group.Items)
                {
                    var name = item.NameVarible;

                    if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(item.Varibles))
                    {

                    }
                    if (item.GhostFormulas.Any())
                    {
                        foreach (var ghostFormula in item.GhostFormulas)
                        {
                            var conditionVaribles = CalculationTools.GetVaribliNames(ghostFormula.Conditions);
                            var dependItems = dataDependItem.Where(x => conditionVaribles.Contains(x.Name)).ToList();
                            if (dependItems.Any())
                            {
                                foreach (var varible in conditionVaribles)
                                {
                                    var items = dataDependItem.Where(x => x.Name == varible).ToList();
                                    if (items.Count > 0)
                                    {
                                        var visibleItems = items.Where(x => x.ReferencedItem.IsVisible).ToList();
                                        if (visibleItems.Any())
                                        {
                                            if (visibleItems.Count() > 1)
                                            {
                                                throw new Exception("On the screen exists two or more a elements with the same 'name'");
                                            }
                                            ghostFormula.DependToItems.Add(new DataDependItem()
                                            {
                                                Name = varible,
                                                IsGhostFormula = true,
                                                IsVisibile = false,
                                                ReferencedItem = visibleItems.FirstOrDefault().ReferencedItem
                                            });
                                            continue;
                                        }
                                        else
                                        {
                                            ghostFormula.DependToItems.Add(new DataDependItem()
                                            {
                                                Name = varible,
                                                IsGhostFormula = true,
                                                IsVisibile = false,
                                                ReferencedItem = items.FirstOrDefault().ReferencedItem
                                            });
                                        }
                                    }
                                }
                            }

                            var dependItem = new DataDependItem()
                            {
                                Name = ghostFormula.Name,
                                IsVisibile = true,
                                IsGhostFormula = true,
                                ReferencedItem = item
                            };
                            // Debug.WriteLine(dependItem.Name);
                            dataDependItem.Add(dependItem);
                        }
                    }

                    if (!string.IsNullOrEmpty(name) && string.IsNullOrEmpty(item.Varibles))
                    {
                        var dependItem = new DataDependItem()
                        {
                            Name = name,
                            IsVisibile = true,
                            ReferencedItem = item
                        };
                        dataDependItem.Add(dependItem);
                    }
                    else if (!string.IsNullOrEmpty(item.Varibles))
                    {
                        var itemsCombo = item.Varibles.Split("|");
                        foreach (var it in itemsCombo)
                        {
                            var dependItem = new DataDependItem()
                            {
                                Name = it,
                                IsVisibile = true,
                                ReferencedItem = item
                            };
                            dataDependItem.Add(dependItem);
                        }
                    }

                }
            }
            return dataDependItem;
        }

        #endregion prepare depend controls

        #region helper methods

        public static List<string> GetTags(string FullLine)
        {
            List<string> taglist = new List<string>();
            int bracketStart, bracketEnd;
            string TagContent;
            int TagLength;

            do
            {
                bracketStart = FullLine.IndexOf("[", StringComparison.Ordinal);
                bracketEnd = FullLine.IndexOf("]", StringComparison.Ordinal);

                TagLength = bracketEnd - bracketStart - 1;
                TagContent = FullLine.Substring(bracketStart + 1, TagLength);
                TagContent = TagContent.Trim();
                taglist.Add(TagContent);
                FullLine = FullLine.Remove(bracketStart, TagLength + 2);
            }
            while (FullLine.IndexOf("[", StringComparison.Ordinal) >= 0);

            return taglist;
        }
        public static String[] StringSplit(String FullLine, string Delimiter)
        {
            List<string> taglist = new List<string>();
            int DelStart;
            string TagContent = FullLine;
            string TagChunk;

            do
            {
                DelStart = TagContent.IndexOf(Delimiter, StringComparison.Ordinal);
                if (DelStart > -1)
                {
                    TagChunk = TagContent.Substring(0, DelStart).Trim();
                    taglist.Add(TagChunk);
                    TagContent = TagContent.Substring(DelStart + 1).Trim();
                }
                else if ((DelStart == -1) && (TagContent.Length > 0))
                {
                    taglist.Add(TagContent.Trim());
                    TagContent = "";
                }
            }
            while (TagContent.Length > 0);

            return taglist.ToArray();
        }


        public static void LogExistsItem(string key, Item item, Item itemExists)
        {
            var infoItem = $"Title : {item.Title}, Group_ID : {item.GroupID}, Item_ID : {item.NumID}, Name : {key}";
            var infoItemExists = $"Title : {itemExists.Title}, Group_ID : {itemExists.GroupID}, Item_ID : {itemExists.NumID}, Name : {itemExists.NameVarible}";
            var resultInfo = $"[{item.Type.ToString().ToUpper()} - {itemExists.Type.ToString().ToUpper()}] - [{infoItem}] - EXISTS ITEM [{infoItemExists}]";
            Debug.WriteLine(resultInfo);
        }
        #endregion helper methods
    }
}
