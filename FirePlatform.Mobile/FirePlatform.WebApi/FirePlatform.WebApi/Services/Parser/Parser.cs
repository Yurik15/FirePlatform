﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FirePlatform.WebApi.Model;

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
                        Items = items
                    };

                    if (!string.IsNullOrEmpty(tag))
                    {
                        groupItems.visCondition = tag.Split(':')[1].Trim();
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
                if (!string.IsNullOrEmpty(group.visCondition))
                {
                    var relatedItems = data.Where(x => group.visCondition.Contains(x.Key.Trim()))
                                            .Select(x => x.Value)
                                            .Distinct()
                                            .ToList();
                    relatedItems.ForEach(x => x.RelatedGroups.Add(group));

                }
                foreach (var item in group.Items)
                {
                    string condition = item.VisCondition ?? string.Empty;
                    if (item.Type == ItemType.Formula.ToString())
                    {
                        if (string.IsNullOrWhiteSpace(item.Formula))
                        {
                            throw new Exception("the item with 'FORMULA' type can't be null or empty!");
                        }
                        var formula = item.GetFormulaString();
                        condition += " " + formula;
                    }
                    if (!string.IsNullOrEmpty(condition))
                    {
                        var relatedItems = data.Where(x => condition.Contains(x.Key.Trim()))
                                                  .Select(x => x.Value)
                                                  .Distinct()
                                                  .ToList();
                        relatedItems.ForEach(x => x.RelatedItems.Add(item));
                        item.RelatedItems.AddRange(relatedItems);
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

        public static string GetFormulaString(this _Item item)
        {
            try
            {
                if (item.Type == ItemType.Formula.ToString())
                {
                    string fullFormula = item.Formula;
                    var parts = fullFormula.Split('=');
                    return parts[1].Trim().Split(' ')[0].Trim();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
    }
}
