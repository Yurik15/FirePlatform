using FirePlatform.WebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Services.Tools
{
    public static class ItemTreeViewExtension
    {
        public static IList<Item> BuildTreeForHtmlItems(this IEnumerable<Item> source)
        {
            var groups = source.GroupBy(i => i.ParentHtmlItemId);

            var roots = groups.FirstOrDefault(g => g.Key.HasValue == false).ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }

        private static void AddChildren(Item node, IDictionary<int, List<Item>> source)
        {
            if (source.ContainsKey(node.NumID))
            {
                node.ChildItems = source[node.NumID];
                for (int i = 0; i < node.ChildItems.Count; i++)
                    AddChildren(node.ChildItems[i], source);
            }
            else
            {
                node.ChildItems = new List<Item>();
            }
        }

        public static IEnumerable<Item> ReadChildNodes(this Item node)
        {
            foreach (Item childNode in node.ChildItems)
            {
                if (childNode.ChildItems != null && childNode.ChildItems.Any())
                {
                    foreach (Item grandChildren in childNode.ReadChildNodes())
                        yield return grandChildren;
                }

                yield return childNode;
            }
        }

    }
}
