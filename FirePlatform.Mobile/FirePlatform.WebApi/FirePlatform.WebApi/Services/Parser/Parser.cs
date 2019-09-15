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
            var varibles = PrepareFieldNameDependToItemDic(controls);//PrepareFieldNameDependToItem(controls);
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
                string MATRIX = "Matrix";
                string MEMOS = "Memos";
                string PICTURE = "Picture";
                string SPACE = " ";
                string COMMENT_LINE = "---";
                string END = "END";
                string COMMA_SEPARATOR = ",";
                char T = '\t';
                char NEW_LINE = '\n';

                var result = new List<ItemGroup>();
                Dictionary<string, List<ComboItem>> Databases = new Dictionary<string, List<ComboItem>>();
                Dictionary<string, string[,]> Matrixes = new Dictionary<string, string[,]>();
                Dictionary<string, string> Memoses = new Dictionary<string, string>();
                Dictionary<String, string> Pictures = new Dictionary<string, string>();
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
                        dbTitle = dbTitle.Split(" ")[0];

                        foreach (var itemText in itemsFromGroup)
                        {
                            var line = itemText.Trim().TrimStart(T);
                            string separator = "\t";
                            if (line.Contains(", \t")) line = line.Replace(", \t", separator);
                            else if (line.Contains(",\t")) line = line.Replace(",\t", separator);
                            else if (line.Contains("\t,")) line = line.Replace("\t,", separator);

                            var dbItems = StringSplit(line, separator); //COMMA_SEPARATOR);

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
                    if (title.StartsWith(PICTURE, StringComparison.Ordinal))
                    {
                        var dbTitle = title.Substring(PICTURE.Length).Trim().TrimStart(T).ToLowerInvariant();
                        var dataOfImage = string.Join(' ', group.Value);
                        Pictures.Add(dbTitle, dataOfImage);
                    }
                    else if (title.StartsWith(MATRIX, StringComparison.Ordinal))
                    {
                        try
                        {
                            var dbTitle = title.Substring(MATRIX.Length).Trim().TrimStart(T).ToLowerInvariant();

                            var columnCount = StringSplit(itemsFromGroup[0], "\t").Length;
                            var rowCount = itemsFromGroup.Count;
                            string[,] matrix = new string[columnCount, rowCount];

                            for (int row = 0; row < rowCount; row++)
                            {
                                var itemText = itemsFromGroup[row];
                                var dbItems = StringSplit(itemText, "\t");
                                for (var column = 0; column < dbItems.Length; column++)
                                {
                                    matrix[column, row] = dbItems[column];
                                }
                            }
                            Matrixes.Add(dbTitle, matrix);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (title.StartsWith(MEMOS, StringComparison.Ordinal))
                    {
                        try
                        {
                            foreach (var memos in itemsFromGroup)
                            {
                                var parts = memos.Split(",");
                                var key = parts[0].Replace("\t", "").Trim();
                                var value = parts[1].Replace("\t", "").Trim();
                                Memoses[key] = value;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                int indexGroup = 0;
                foreach (var group in data)
                {
                    var title = group.Key.Item1;
                    var tag = group.Key.Item2;
                    var itemsFromGroup = group.Value;

                    if (title.StartsWith(DATABASE, StringComparison.Ordinal) || title.StartsWith(MATRIX, StringComparison.Ordinal) || title.StartsWith(MEMOS, StringComparison.Ordinal) || title.StartsWith(PICTURE, StringComparison.Ordinal)) continue;

                    var items = new List<Item>();
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

                        var item = ItemHelper.Prepare(itemText, indexItems, indexGroup, title, tag, Databases, Matrixes, Memoses, Pictures);
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
        private static void PrepareGroupDependToItems(List<ItemGroup> _ItemGroups, Dictionary<string, List<DataDependItem>> data)
        {

            //-------DON'T REMOVE
            /*foreach (var a in data)
            {
                if (a.Name.Contains("|") || a.Name.Contains(",") || a.Name.Contains("."))
                {
                    Debug.WriteLine($"TYPE : {a.ReferencedItem.Type}, GROUP_ID : {a.ReferencedItem.GroupID}, NUM_ID : {a.ReferencedItem.NumID}, VALUE : {a.Name}");
                }
            }*/

            //Calculate items
            foreach (var group in _ItemGroups)
            {
                foreach (var item in group.Items)
                {
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
                        var relatedItems = new List<KeyValuePair<string, List<DataDependItem>>>();
                        foreach (var varibleName in item.VisConditionNameVaribles)
                        {
                            var elmentesBeforCurrentElement = data;
                            /*var elmentesBeforCurrentElement = data.Where(x => x.ReferencedItem.NumID < item.NumID && x.ReferencedItem.GroupID == item.GroupID).ToList();
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
                            }*/

                            foreach (var dependElement in elmentesBeforCurrentElement)
                            {
                                var needToAdd = false;
                                var nameDependElement = dependElement.Key;
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
                        relatedItems.ForEach(x => x.Value.ForEach(y => y.ReferencedItem.NeedNotifyItems.Add(item)));

                    }

                    if (!string.IsNullOrEmpty(item.Formula))
                    {
                        var relatedItems = new List<KeyValuePair<string, List<DataDependItem>>>();
                        foreach (var varibleName in item.FormulaNameVaribles)
                        {
                            var elmentesBeforCurrentElement = data;
                            /*var elmentesBeforCurrentElement = data.Where(x => x.ReferencedItem.NumID < item.NumID && x.ReferencedItem.GroupID == item.GroupID).ToList();
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
                            }*/

                            foreach (var dependElement in elmentesBeforCurrentElement)
                            {
                                var needToAdd = false;
                                var nameDependElement = dependElement.Key;
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
                        relatedItems.ForEach(x => x.Value.ForEach(y => y.ReferencedItem.NeedNotifyItems.Add(item)));

                        if (item.ParentGroup.IsVisible) // PERFORMANCE
                        {
                            var paramsDic = ItemExtentions.GetParams(item.DependToItemsForFormulas);
                            object result = null;
                            if (item.Matrix != null)
                            {
                                result = CalculationTools.CalculateFormulasMatrix(item.Formula, item.Matrix, paramsDic);
                            }
                            else
                            {
                                result = CalculationTools.CalculateFormulas(item.Formula, paramsDic);
                            }
                            item.Value = result;
                        }
                    }
                    if (!string.IsNullOrEmpty(item.VisCondition))
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
            //Calculate groups
            foreach (var group in _ItemGroups)
            {
                if (!string.IsNullOrEmpty(group.VisCondition))
                {
                    var relatedItems = new List<KeyValuePair<string, List<DataDependItem>>>();
                    foreach (var varibleName in group.VisConditionNameVaribles)
                    {
                        foreach (var dependElement in data)
                        {
                            var needToAdd = false;
                            var nameDependElement = dependElement.Key;
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
                    relatedItems.ForEach(x => x.Value.ForEach(y => y.ReferencedItem.NeedNotifyGroups.Add(group)));

                    var paramsDic = ItemExtentions.GetParams(group.DependToItems);
                    var res = CalculationTools.CalculateVis(group.VisCondition, paramsDic);
                    group.IsVisible = res.HasValue ? res.Value : false;
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
                    if (!string.IsNullOrWhiteSpace(item.NameVaribleMatrix))
                    {
                        var dependItem = new DataDependItem()
                        {
                            Name = item.NameVaribleMatrix,
                            ReferencedItem = item
                        };
                        dataDependItem.Add(dependItem);
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
                            IsVisibile = false,
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
                                IsVisibile = false,
                                ReferencedItem = item
                            };
                            dataDependItem.Add(dependItem);
                        }
                    }
                }
            }
            return dataDependItem;
        }

        private static Dictionary<string, List<DataDependItem>> PrepareFieldNameDependToItemDic(List<ItemGroup> _ItemGroups)
        {
            var dependItems = PrepareFieldNameDependToItem(_ItemGroups);
            var groupedItemsByName = dependItems.GroupBy(x => x.Name).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());
            //var a = groupedItemsByName.Where(x => x.Value.Count > 1).OrderBy(x => x.Value.Count).ToDictionary(gdc => gdc.Key, gdc => gdc.Value);
            return groupedItemsByName;
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
