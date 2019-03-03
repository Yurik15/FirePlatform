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
        public static List<_ItemGroup> PrepareControls(string fileContent)
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

            var a = varibles.Where(x => x.Name.Trim().ToLower() == "hhs33").ToList();

            startDate = DateTime.Now;
            PrepareGroupDependToItems(controls, varibles);
            endDate = DateTime.Now;
            result = endDate - startDate;
            Debug.WriteLine($"[ParseDoc] - Time - minutes : {result.Minutes} or seconds : {result.Seconds}");

            return controls;
        }

        #region parser
        private static List<_ItemGroup> ParseDoc(string fileContent)
        {
            string DATABASE = "Database";
            string SPACE = " ";
            string COMMENT_LINE = "---";
            string END = "END";
            string COMMA_SEPARATOR = ",";
            char T = '\t';
            char NEW_LINE = '\n';

            var result = new List<_ItemGroup>();
            Dictionary<string, List<ComboItem>> Databases = new Dictionary<string, List<ComboItem>>();

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
                        var dbItems = StringSplit(line, COMMA_SEPARATOR);

                        var displayName = dbItems[0];
                        var groupKey = String.Join(",", dbItems.Skip(1));
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

                var items = new List<_Item>();

                if ("picture".Equals(group.Key.Item1.Split(' ')[0].ToLower()))
                {
                    var item = ItemHelper.PreparePicture(group.Key.Item1, itemsFromGroup);
                    //result.Add(new ItemGroup(indexGroup, title, tag, item));
                }
                else
                {
                    var groupItems = new _ItemGroup
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
        #endregion parser

        #region prepare depend controls
        private static void PrepareGroupDependToItems(List<_ItemGroup> _ItemGroups, List<DataDependItem> data)
        {
            foreach (var group in _ItemGroups)
            {
                /*if (!string.IsNullOrEmpty(group.VisCondition))
                {
                    var relatedItems = data.Where(x => group.VisCondition.Contains(x.Name.Trim()) && !string.IsNullOrEmpty(x.Name.Trim()))
                                            .Select(x => x.ReferencedItem)
                                            .ToList();
                    var relatedToSelectedList = data.Where(x => string.IsNullOrEmpty(x.Name.Trim()) && !string.IsNullOrEmpty(x.ReferencedItem.Varibles))
                                                        .ToList();
                    if (relatedToSelectedList.Any())
                    {
                        foreach (var itemList in relatedToSelectedList)
                        {
                            var itemsList = itemList.ReferencedItem.Varibles.Split('|').ToList();
                            if (itemsList.Any(x => group.VisCondition.Contains(x)))
                            {
                                relatedItems.Add(itemList.ReferencedItem);
                            }
                        }
                    }
                    group.DependToItems = relatedItems;
                    relatedItems.ForEach(x => x.NeedNotifyGroups.Add(group));
                }*/
                foreach (var item in group.Items)
                {
                    bool isFormula = false;
                    bool isVisibleConditions = false;
                    string condition = item.VisCondition ?? string.Empty;
                    /*if (item.Type == ItemType.Formula.ToString())
                    {
                        isFormula = true;
                        if (string.IsNullOrWhiteSpace(item.Formula))
                        {
                            throw new Exception("the item with 'FORMULA' type can't be null or empty!");
                        }
                        var formula = item.GetFormulaString();
                        condition += " " + formula;
                    }*/
                    if (!string.IsNullOrEmpty(condition))
                    {
                        isVisibleConditions = true;
                        var relatedItems = new List<_Item>();
                        foreach (var varibleName in item.VisConditionNameVaribles)
                        {
                            var foundItems = data.Where(x => x.Name.Trim().ToLower() == varibleName.Trim().ToLower());
                            if (foundItems == null)
                            {
                                Debug.WriteLine($"{varibleName} - not found");
                            }
                            else
                            {
                                if(foundItems.Count() == 1)
                                {
                                    var foundItem = foundItems.FirstOrDefault();
                                    var alreadyExists = relatedItems.FirstOrDefault(x => x.GroupID == foundItem.ReferencedItem.GroupID && x.NumID == foundItem.ReferencedItem.NumID);
                                    if(alreadyExists == null)
                                    {
                                        relatedItems.Add(foundItem.ReferencedItem);
                                    }
                                }
                                else
                                {
                                    foreach(var foundItem in foundItems)
                                    {
                                        var alreadyExists = relatedItems.FirstOrDefault(x => x.GroupID == foundItem.ReferencedItem.GroupID && x.NumID == foundItem.ReferencedItem.NumID);
                                        if (alreadyExists == null)
                                        {
                                            relatedItems.Add(foundItem.ReferencedItem);
                                        }
                                    }
                                }
                            }
                        }
                        item.DependToItems = relatedItems;
                        relatedItems.ForEach(x => x.NeedNotifyItems.Add(item));

                    }
                    /*if (isFormula)
                    {
                        var paramsDic = item.GetParams();
                        var formula = item.GetFormulaString();
                        item.Value = CalculationTools.Calculate(formula, paramsDic);
                    }*/
                    if (isVisibleConditions)
                    {
                        var paramsDic = item.GetParams();
                        var res = CalculationTools.CalculateVis(item.VisCondition, paramsDic);
                        item.IsVisible = res.HasValue ? res.Value : false;
                    }
                }
            }
        }

        private static List<DataDependItem> PrepareFieldNameDependToItem(List<_ItemGroup> _ItemGroups)
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
                            var dependItem = new DataDependItem()
                            {
                                Name = ghostFormula.Name,
                                IsVisibile = true,
                                IsGhostFormula = true,
                                ReferencedItem = item
                            };
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


        public static void LogExistsItem(string key, _Item item, _Item itemExists)
        {
            var infoItem = $"Title : {item.Title}, Group_ID : {item.GroupID}, Item_ID : {item.NumID}, Name : {key}";
            var infoItemExists = $"Title : {itemExists.Title}, Group_ID : {itemExists.GroupID}, Item_ID : {itemExists.NumID}, Name : {itemExists.NameVarible}";
            var resultInfo = $"[{item.Type.ToString().ToUpper()} - {itemExists.Type.ToString().ToUpper()}] - [{infoItem}] - EXISTS ITEM [{infoItemExists}]";
            Debug.WriteLine(resultInfo);
        }
        #endregion helper methods
    }
}
