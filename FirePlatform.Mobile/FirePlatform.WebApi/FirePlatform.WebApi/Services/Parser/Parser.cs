using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FirePlatform.WebApi.Model;
using FirePlatform.WebApi.Services.Tools;

namespace FirePlatform.WebApi.Services.Parser
{
    public static class Parser
    {
        public static List<_ItemGroup> ParseDoc(string fileContent)
        {
            string DATABASE = "Database";
            string SPACE = " ";
            string COMMENT_LINE = "---";
            string END = "END";
            string SEPARATOR = "|";
            string COMMA_SEPARATOR = ",";
            char T = '\t';
            char NEW_LINE = '\n';

            var result = new List<_ItemGroup>();
            Dictionary<string, List<string>> Databases = new Dictionary<string, List<string>>();

            string[] initLines = fileContent.Split(NEW_LINE);


            Dictionary<Tuple<string, string>, List<string>> data = new Dictionary<Tuple<string, string>, List<string>>();
            foreach (var line in initLines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith(COMMENT_LINE)) continue;

                if (line.StartsWith(SPACE) || line.StartsWith(T.ToString()))
                {
                    data.Last().Value.Add(line.Trim());
                }
                else
                {
                    string tag = string.Empty;
                    string title = line.Trim();

                    var bracket = line.IndexOf("[");
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

                if (title.StartsWith(DATABASE))
                {
                    var tempDB = new List<string>();
                    var txtLine = title.Substring(DATABASE.Length).Trim().TrimStart(T);
                    var names = StringSplit(txtLine, SPACE);
                    var dbTitle = names.First().ToLowerInvariant();
                    var namesList = names.Skip(1).ToList();

                    var countOfNames = namesList.Count();

                    foreach (var itemText in itemsFromGroup)
                    {
                        var line = itemText.Trim().TrimStart(T);
                        var dbItems = StringSplit(line, COMMA_SEPARATOR);
                        var dbItemsList = new List<string>(dbItems);

                        for (int index = 0; index < dbItems.Count() - 1; index++)
                        {
                            if (index < countOfNames)
                            {
                                dbItemsList.Add(namesList[index].Trim().ToLowerInvariant());
                                dbItemsList.Add(dbItems[index + 1].Trim().ToLowerInvariant());
                            }
                            else
                            {
                                foreach (var dbtag in StringSplit(dbItems[index + 1], SEPARATOR))
                                {
                                    dbItemsList.Add(dbtag.ToLowerInvariant());
                                    dbItemsList.Add(bool.TrueString);
                                }
                            }
                        }
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

                if (title.StartsWith(DATABASE)) continue;

                var items = new List<_Item>();

                if ("picture".Equals(group.Key.Item1.Split(' ')[0].ToLower()))
                {
                    var item = ItemHelper.PreparePicture(group.Key.Item1, itemsFromGroup);
                    //result.Add(new ItemGroup(indexGroup, title, tag, item));
                }
                else
                {

                    for (int indexItems = 0; indexItems < itemsFromGroup.Count; indexItems++)
                    {
                        var itemText = itemsFromGroup[indexItems];
                        if (itemText.StartsWith(END)) break;
                        else if (string.IsNullOrWhiteSpace(itemText) || itemText.StartsWith(COMMENT_LINE)) continue;

                        var item = ItemHelper.Prepare(itemText, indexItems, indexGroup, title, tag, Databases);

                        items.Add(item);
                    }
                    var groupItems = new _ItemGroup
                    {
                        IndexGroup = indexGroup,
                        Title = title,
                        Tag = tag,
                        Items = items,
                        IsVisible = true,
                        InitialVisibility = true
                    };

                    if (!string.IsNullOrEmpty(tag))
                    {
                        groupItems.VisCondition = tag.Split(':')[1].Trim();
                        groupItems.IsVisible = false;
                        groupItems.InitialVisibility = false;
                    }


                    result.Add(groupItems);
                    indexGroup++;
                }
            }

            var varibles = PrepareFieldNameDependToItem(result);

            PrepareGroupDependToItems(result, varibles);

            return result;
        }

        private static void PrepareGroupDependToItems(List<_ItemGroup> _ItemGroups, Dictionary<string, _Item> data)
        {
            foreach (var group in _ItemGroups)
            {
                if (!string.IsNullOrEmpty(group.VisCondition))
                {
                    var relatedItems = data.Where(x => group.VisCondition.Contains(x.Key.Trim()) && !string.IsNullOrEmpty(x.Key.Trim()))
                                            .Select(x => x.Value)
                                            .Distinct()
                                            .ToList();
                    var relatedToSelectedList = data.Where(x => string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.Value.Varibles))
                                                        .ToList();
                    if (relatedToSelectedList.Any())
                    {
                        foreach (var itemList in relatedToSelectedList)
                        {
                            var itemsList = itemList.Value.Varibles.Split('|').ToList();
                            if (itemsList.Any(x => group.VisCondition.Contains(x)))
                            {
                                relatedItems.Add(itemList.Value);
                            }
                        }
                    }
                    group.DependToItems = relatedItems;
                    relatedItems.ForEach(x => x.NeedNotifyGroups.Add(group));
                }
                foreach (var item in group.Items)
                {
                    bool isFormula = false;
                    bool isVisibleConditions = false;
                    string condition = item.VisCondition ?? string.Empty;
                    if (item.Type == ItemType.Formula.ToString())
                    {
                        isFormula = true;
                        if (string.IsNullOrWhiteSpace(item.Formula))
                        {
                            throw new Exception("the item with 'FORMULA' type can't be null or empty!");
                        }
                        var formula = item.GetFormulaString();
                        condition += " " + formula;
                    }
                    if (!string.IsNullOrEmpty(condition))
                    {
                        isVisibleConditions = true;
                        var relatedItems = data.Where(x => !string.IsNullOrEmpty(x.Key) && condition.Contains(x.Key.Trim()))
                                                  .Select(x => x.Value)
                                                  .Distinct()
                                                  .ToList();
                        var relatedToSelectedList = data.Where(x => string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.Value.Varibles))
                                                        .ToList();
                        if (relatedToSelectedList.Any())
                        {
                            foreach (var itemList in relatedToSelectedList)
                            {
                                var itemsList = itemList.Value.Varibles.Split('|').ToList();
                                if (itemsList.Any(x => condition.Contains(x)))
                                {
                                    relatedItems.Add(itemList.Value);
                                }
                            }
                        }
                        item.DependToItems = relatedItems;
                        relatedItems.ForEach(x => x.NeedNotifyItems.Add(item));

                    }
                    if (isFormula)
                    {
                        var paramsDic = item.GetParams();
                        var formula = item.GetFormulaString();
                        item.Value = CalculationTools.Calculate(formula, paramsDic);
                    }
                    if (isVisibleConditions)
                    {
                        var paramsDic = item.GetParams();
                        var res = CalculationTools.CalculateVis(item.VisCondition, paramsDic);
                        item.IsVisible = res.HasValue ? res.Value : false;
                    }
                }
            }
        }

        private static Dictionary<string, _Item> PrepareFieldNameDependToItem(List<_ItemGroup> _ItemGroups)
        {
            Dictionary<string, _Item> data = new Dictionary<string, _Item>();
            foreach (var group in _ItemGroups)
            {
                foreach (var item in group.Items)
                {
                    var name = item.NameVarible;
                    if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(item.Varibles))
                    {
                        if (!data.ContainsKey(name))
                        {
                            data.Add(name, item);
                        }
                        else
                        {
                            Debug.WriteLine($"[{item.GroupID}-{item.NumID}]Name - {name} | [{data[name].GroupID}-{data[name].NumID}]Name - {name}");
                        }
                    }

                }
            }
            return data;
        }

        public static List<string> GetTags(string FullLine)
        {
            List<string> taglist = new List<string>();
            int bracketStart, bracketEnd;
            string TagContent;
            int TagLength;

            do
            {
                bracketStart = FullLine.IndexOf("[");
                bracketEnd = FullLine.IndexOf("]");

                TagLength = bracketEnd - bracketStart - 1;
                TagContent = FullLine.Substring(bracketStart + 1, TagLength);
                TagContent = TagContent.Trim();
                taglist.Add(TagContent);
                FullLine = FullLine.Remove(bracketStart, TagLength + 2);
            }
            while (FullLine.IndexOf("[") >= 0);

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
                DelStart = TagContent.IndexOf(Delimiter);
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
    }
}
