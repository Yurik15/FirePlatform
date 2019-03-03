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
            if (item.ParentGroup.IsVisible)
            {
                var paramsDic = item.GetParams();
                var formula = item.GetFormulaString();
                if (!paramsDic.Where(x => string.IsNullOrEmpty(x.Key)).Any())
                {
                    if (!string.IsNullOrEmpty(item.VisCondition) && !onlyFormula)
                    {
                        //var resultValue = CalculationTools.CalculateVis(item.VisCondition, paramsDic);
                        //if (resultValue.HasValue)
                        //    item.IsVisible = resultValue.Value;
                    }
                    if (!string.IsNullOrEmpty(formula) && item.IsVisible)
                    {
                        //var resultValue = CalculationTools.Calculate(formula, paramsDic);
                        //item.Value = resultValue;
                    }
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
                    //var resultValue = CalculationTools.CalculateVis(itemGroup.VisCondition, paramsDic);
                    //if (resultValue.HasValue)
                    //    itemGroup.IsVisible = resultValue.Value;
                }
            }
        }

        public static Dictionary<string, object> GetParamsForVisCondition(this _Item item)
        {
            var paramsDic = new Dictionary<string, object>();
            foreach(var relatedItem in item.DependToItems)
            {
                try
                {
                    if (relatedItem.Type == ItemType.Combo.ToString())
                    {

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[GetParamsForVisCondition] {relatedItem.NameVarible} - {ex.Message} \n");
                }
            }
            return paramsDic;
        }

        public static Dictionary<string, object> GetParams(this _Item item)
        {
            if(item.NumID == 4)
            {

            }
            var paramsDic = new Dictionary<string, object>();

            var onlyVisibleItem = item.DependToItems.Where(x => x.IsVisible).ToList();

            foreach (var relatedItem in onlyVisibleItem)
            {
                try
                {
                    if (relatedItem.Type == ItemType.Combo.ToString())
                    {
                        var itemsFromCombo = relatedItem.Varibles;
                        if (!string.IsNullOrEmpty(itemsFromCombo))
                        {
                            var listVarible = itemsFromCombo.Split('|').ToDictionary(x => x, y => false);
                            if (!string.IsNullOrEmpty(relatedItem.NameVarible))
                            {
                                listVarible[relatedItem.NameVarible] = bool.Parse(relatedItem.Value);
                            }
                            foreach (var itemCombo in listVarible)
                            {
                                if (paramsDic.ContainsKey(itemCombo.Key))
                                {
                                    paramsDic[itemCombo.Key] = itemCombo.Value;
                                }
                                else
                                {
                                    paramsDic.Add(itemCombo.Key.Trim().ToLower(), itemCombo.Value);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!paramsDic.ContainsKey(relatedItem.NameVarible))
                        {
                            paramsDic.Add(relatedItem.NameVarible.Trim().ToLower(), relatedItem.Value);
                        }
                        else
                        {
                            var param = paramsDic[relatedItem.NameVarible];
                            //Params exists
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[GetParams] {relatedItem.NameVarible} - {ex.Message} \n");
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
