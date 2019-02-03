using System;
using System.Collections.Generic;
using System.Linq;
using FirePlatform.WebApi.Model;

namespace FirePlatform.WebApi.Services.Tools
{
    public static class ItemExtentions
    {
        public static void NotifyAboutChange(this _Item item)
        {
            foreach (var relatedGroup in item.NeedNotifyGroups)
            {
                relatedGroup.UpdateGroup();
            }
            foreach (var relatedItem in item.NeedNotifyItems)
            {
                relatedItem.UpdateItem();
            }
        }
        public static void UpdateItem(this _Item item, bool onlyFormula = false)
        {
            var paramsDic = item.GetParams();
            var formula = item.GetFormulaString();
            if (!paramsDic.Where(x => string.IsNullOrEmpty(x.Key)).Any())
            {
                if (!string.IsNullOrEmpty(formula))
                {
                    var resultValue = CalculationTools.Calculate(formula, paramsDic);
                    item.Value = resultValue;
                }
                if (!string.IsNullOrEmpty(item.VisCondition) && !onlyFormula)
                {
                    var resultValue = CalculationTools.CalculateVis(item.VisCondition, paramsDic);
                    if (resultValue.HasValue)
                        item.IsVisible = resultValue.Value;
                }
            }

        }
        public static void UpdateGroup(this _ItemGroup itemGroup)
        {
            var paramsDic = itemGroup.GetParams();
            if (!paramsDic.Where(x => string.IsNullOrEmpty(x.Key)).Any())
            {
                if (!string.IsNullOrEmpty(itemGroup.VisCondition))
                {
                    var resultValue = CalculationTools.CalculateVis(itemGroup.VisCondition, paramsDic);
                    if (resultValue.HasValue)
                        itemGroup.IsVisible = resultValue.Value;
                }
            }
        }
        public static Dictionary<string, object> GetParams(this _Item item)
        {
            var paramsDic = new Dictionary<string, object>();
            foreach (var relatedItem in item.DependToItems)
            {
                //if (relatedItem.Type == ItemType.Formula.ToString())
                //{
                //relatedItem.UpdateItem(true);
                //}
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
            foreach (var relatedItem in item.DependToItems)
            {
                if (!paramsDic.ContainsKey(relatedItem.NameVarible))
                {
                    paramsDic.Add(relatedItem.NameVarible, relatedItem.Value);
                }
            }
            return paramsDic;
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
