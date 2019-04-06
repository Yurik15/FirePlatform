using System;
using System.Collections.Generic;
using System.Linq;
using FirePlatform.WebApi.Model;
using FirePlatform.WebApi.Model.Template;
using NCalc;

namespace FirePlatform.WebApi.Services.Tools
{
    public static class ItemExtentions
    {
        public static void NotifyAboutChange(this Item item)
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
        public static void UpdateItem(this Item item)
        {
            if (item.ParentGroup.IsVisible)
            {
                var formula = item.Formula;
                var visCondition = item.VisCondition;
                if (!string.IsNullOrEmpty(visCondition))
                {
                    var paramsDic = ItemExtentions.GetParams(item.DependToItems);
                    var res = CalculationTools.CalculateVis(item.VisCondition, paramsDic);
                    item.IsVisible = res.HasValue ? res.Value : false;
                }
                if (!string.IsNullOrEmpty(formula) && item.IsVisible)
                {
                    var paramsDic = ItemExtentions.GetParams(item.DependToItemsForFormulas);
                    var res = CalculationTools.CalculateFormulas(formula, paramsDic);
                    item.Value = res;
                }
            }
        }
        public static void UpdateGroup(this ItemGroup itemGroup)
        {

            if (!string.IsNullOrEmpty(itemGroup.VisCondition))
            {
                var paramsDic = ItemExtentions.GetParams(itemGroup.DependToItems);
                var res = CalculationTools.CalculateVis(itemGroup.VisCondition, paramsDic);
                itemGroup.IsVisible = res.HasValue ? res.Value : false;
            }
        }

        public static Dictionary<string, object> GetParams(List<DataDependItem> DependToItems)
        {
            var paramsDic = new Dictionary<string, object>();
            foreach (var relatedItem in DependToItems)
            {
                if (relatedItem.ReferencedItem.Type == ItemType.Combo.ToString())
                {
                    var name = relatedItem.Name;
                    var value = false;
                    if (paramsDic.ContainsKey(name.Trim()))
                    {

                    }
                    else
                    {
                        var nameVarible = relatedItem.ReferencedItem.NameVarible;
                        if (name.Contains(","))
                        {
                            var names = name.Split(",");

                            foreach (var nm in names)
                            {
                                if (paramsDic.ContainsKey(nm.Trim()))
                                {

                                }
                                else
                                {
                                    value = nameVarible == nm;
                                    paramsDic.Add(nm.Trim(), value);
                                }
                            }
                        }
                        else
                        {
                            value = nameVarible == name;
                        }
                        if (!name.Contains(","))
                            paramsDic.Add(name.Trim(), value);
                    }
                }
                else if (relatedItem.ReferencedItem.Type == ItemType.Text.ToString())
                {
                    if (paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible.Trim()))
                    {

                    }
                    else
                    {
                        paramsDic.Add(relatedItem.ReferencedItem.NameVarible.Trim(), relatedItem.ReferencedItem.Value ?? string.Empty);
                    }
                }
                else if (relatedItem.ReferencedItem.Type == ItemType.Num.ToString())
                {
                    if (paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible.Trim()))
                    {

                    }
                    else
                    {
                        paramsDic.Add(relatedItem.ReferencedItem.NameVarible.Trim(), relatedItem.ReferencedItem.Value ?? -9999999);
                    }
                }
                else if (relatedItem.ReferencedItem.Type == ItemType.Check.ToString())
                {
                    if (paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible.Trim()))
                    {

                    }
                    else
                    {
                        paramsDic.Add(relatedItem.ReferencedItem.NameVarible.Trim(), relatedItem.ReferencedItem.Value ?? false);
                    }
                }
                else if (relatedItem.ReferencedItem.Type == ItemType.Formula.ToString())
                {
                    if (paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible.Trim()))
                    {

                    }
                    else
                    {
                        var param = GetParamsFromFormulasAfterCalculation(relatedItem);
                        paramsDic.Add(param.name.Trim(), param.value ?? false);
                    }
                }
                if (relatedItem.ReferencedItem.GhostFormulas.Any())
                {
                    var paramsDicFromGhostFormulas = GetParamsFromGhostFormulas(relatedItem.ReferencedItem.GhostFormulas, relatedItem.ReferencedItem.IsVisible && relatedItem.ReferencedItem.IsGroupVisible);
                    foreach (var element in paramsDicFromGhostFormulas)
                    {
                        if (paramsDic.ContainsKey(element.Key.Trim()))
                        {

                        }
                        else
                        {
                            paramsDic.Add(element.Key.Trim(), element.Value);
                        }
                    }
                }
            }
            return paramsDic;
        }

        #region formulas

        private static (string name, object value) GetParamsFromFormulasAfterCalculation(DataDependItem dataDependItem)
        {
            var item = dataDependItem.ReferencedItem;
            var param = GetParams(item.DependToItemsForFormulas);//var param = GetParamsFromFormulas(item);
            var value = CalculationTools.CalculateFormulas(item.Formula, param);
            return (dataDependItem.Name, value);
        }

        private static Dictionary<string, object> GetParamsFromFormulas(Item item)
        {
            var paramsDic = new Dictionary<string, object>();
            try
            {
                foreach (var relatedItem in item.DependToItemsForFormulas)
                {
                    var nameVarible = relatedItem.Name;
                    if (!string.IsNullOrEmpty(nameVarible) && !paramsDic.ContainsKey(nameVarible))
                    {
                        var value = relatedItem.ReferencedItem.Value;
                        if (string.IsNullOrEmpty(nameVarible))
                        {
                            nameVarible = relatedItem.Name;
                        }
                        if (value == null || string.IsNullOrEmpty(value as string))
                        {
                            var type = relatedItem.ReferencedItem.Type;
                            value = GetDefaultValueForElement(type);
                        }
                        paramsDic.Add(nameVarible, value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return paramsDic;
        }
        #endregion formulas

        #region ghost formulas
        private static Dictionary<string, object> GetParamsFromGhostFormulas(List<GhostFormula> ghostFormulas, bool itemIsVisible)
        {
            var paramsDic = new Dictionary<string, object>();
            foreach (var ghostFormula in ghostFormulas)
            {
                if (ghostFormula.DependToItems.Any())
                {
                    var paramsGhostFormula = ghostFormula.GetParams();
                    var value = CalculationTools.CalculateFormulas(ghostFormula.Conditions, paramsGhostFormula); //TODO need to check if element is visible but the return type is unknown and we need set the element any value (if not set then app throw exception)
                    paramsDic.Add(ghostFormula.Name, value);
                }
                else if(!paramsDic.ContainsKey(ghostFormula.Name))
                {
                    paramsDic.Add(ghostFormula.Name, ghostFormula.Conditions);
                }
            }
            return paramsDic;
        }
        public static Dictionary<string, object> GetParams(this GhostFormula ghostFormula)
        {
            var paramsDic = new Dictionary<string, object>();
            try
            {
                foreach (var relatedItem in ghostFormula.DependToItems)
                {
                    if (!paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible))
                    {
                        var nameVarible = relatedItem.ReferencedItem.NameVarible;
                        var value = relatedItem.ReferencedItem.Value;
                        if (string.IsNullOrEmpty(nameVarible))
                        {
                            nameVarible = relatedItem.Name;
                        }
                        if (value == null || string.IsNullOrEmpty(value as string))
                        {
                            var type = relatedItem.ReferencedItem.Type;
                            value = GetDefaultValueForElement(type);
                        }
                        paramsDic.Add(nameVarible, value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return paramsDic;
        }


        #endregion ghost formulas

        #region helpers
        private static dynamic GetDefaultValueForElement(string type)
        {
            dynamic value = null;
            if (type == ItemType.Formula.ToString())
            {
                value = " "; //TODO need to calculate value
            }
            else if (type == ItemType.Num.ToString())
            {
                value = -9999999;
            }
            else if (type == ItemType.Check.ToString() || type == ItemType.Combo.ToString())
            {
                value = false;
            }
            else if (type == ItemType.Text.ToString())
            {
                value = " ";
            }
            return value;
        }
        #endregion helpers

        /*public static Dictionary<string, object> GetParams(this Item item)
        {
            var paramsDic = new Dictionary<string, object>();

            foreach (var relatedItem in item.DependToItems)
            {
                try
                {
                    if (relatedItem.ReferencedItem.Type == ItemType.Combo.ToString())
                    {
                        var itemsFromCombo = relatedItem.ReferencedItem.Varibles;
                        if (!string.IsNullOrEmpty(itemsFromCombo))
                        {
                            Dictionary<string, bool> listVarible = new Dictionary<string, bool>();
                            try
                            {
                                Func<string, bool> funcWhere = (string itemCombo) =>
                                {
                                    var namesVar = item.VisConditionNameVaribles;
                                    var currentItem = itemCombo;
                                    if (currentItem.IndexOf(',') == -1)
                                    {
                                        return namesVar.Contains(currentItem);
                                    }
                                    else
                                    {
                                        var partsOfItemCombo = currentItem.Split(',');
                                        foreach (var it in partsOfItemCombo)
                                        {
                                            if (namesVar.Contains(it.Trim())) return true;
                                        }
                                    }
                                    return false;
                                };



                                var listVar = itemsFromCombo.Split('|').Where(x => funcWhere(x));
                                foreach (var it in listVar)
                                {
                                    if (!listVarible.ContainsKey(it))
                                    {
                                        listVarible.Add(it, false);
                                    }
                                    else
                                    {
                                        //System.Diagnostics.Debug.WriteLine($"(0)[GetParams] {relatedItem.NameVarible}\n");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"(1)[GetParams] {relatedItem.ReferencedItem.NameVarible} - {ex.Message} \n");
                            }
                            if (!string.IsNullOrEmpty(relatedItem.ReferencedItem.NameVarible))
                            {
                                listVarible[relatedItem.ReferencedItem.NameVarible] = bool.Parse(relatedItem.ReferencedItem.Value);
                            }
                            foreach (var itemCombo in listVarible)
                            {
                                if (paramsDic.ContainsKey(itemCombo.Key))
                                {
                                    paramsDic[itemCombo.Key] = itemCombo.Value;
                                }
                                else
                                {
                                    var key = itemCombo.Key;
                                    if (paramsDic.ContainsKey(key))
                                    {
                                        var it = item.DependToItems.Where(x => x.ReferencedItem.Varibles.Split('|').Contains(key)).ToList();
                                        it = it.Where(x => x.ReferencedItem.IsVisible).ToList();
                                        if (it.Count == 1)
                                        {
                                            if (it.FirstOrDefault().ReferencedItem.NameVarible == key)
                                                paramsDic[key] = true;
                                        }
                                        else if (it.Count > 1)
                                        {
                                            throw new Exception("Custom Exception - two or more items with the same name are visbile!");
                                        }

                                    }
                                    else
                                    {
                                        paramsDic.Add(itemCombo.Key, itemCombo.Value);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(relatedItem.ReferencedItem.NameVarible))
                        {
                            if (!paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible))
                            {
                                if (relatedItem.ReferencedItem.Type == ItemType.Text.ToString())
                                {
                                    paramsDic.Add(relatedItem.ReferencedItem.NameVarible, relatedItem.ReferencedItem.Value ?? string.Empty);
                                }
                                else if (relatedItem.ReferencedItem.Type == ItemType.Num.ToString())
                                {
                                    paramsDic.Add(relatedItem.ReferencedItem.NameVarible, relatedItem.ReferencedItem.Value ?? "0");
                                }
                                else
                                {
                                    paramsDic.Add(relatedItem.ReferencedItem.NameVarible, relatedItem.ReferencedItem.Value ?? "0");
                                }
                            }
                            else
                            {
                                var param = paramsDic[relatedItem.ReferencedItem.NameVarible];
                                //Params exists
                            }
                        }
                        else
                        {
                            //System.Diagnostics.Debug.WriteLine($"(2)[GetParams] {relatedItem.NameVarible} - type : {relatedItem.Type} \n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"(3)[GetParams] {relatedItem.ReferencedItem.NameVarible} - {ex.Message} \n");
                }
            }
            if (paramsDic.Count != item.VisConditionNameVaribles.Count)
            {

            }

            Func<string, string> funcSelect = (string itemCombo) =>
            {
                var namesVar = item.VisConditionNameVaribles;
                var currentItem = itemCombo;
                if (currentItem.IndexOf(',') > -1)
                {
                    var partsOfItemCombo = currentItem.Split(',');
                    foreach (var it in partsOfItemCombo)
                    {
                        if (namesVar.Contains(it.Trim())) return it.Trim();
                    }
                }
                return null;
            };

            var newParamsDic = new Dictionary<string, object>();

            foreach (var param in paramsDic)
            {
                if (param.Key.IndexOf(',') > -1)
                {
                    var newKey = funcSelect(param.Key);
                    if (!newParamsDic.ContainsKey(newKey))
                    {
                        newParamsDic.Add(newKey, param.Value);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (newParamsDic.ContainsKey(param.Key))
                    {

                    }
                    else
                    {
                        newParamsDic.Add(param.Key, param.Value);
                    }
                }
            }

            return newParamsDic;
        }*/
        public static Dictionary<string, object> GetParams(this ItemGroup item)
        {
            var paramsDic = new Dictionary<string, object>();
            foreach (var relatedItem in item.DependToItems)
            {
                /*if (!paramsDic.ContainsKey(relatedItem.NameVarible))
                {
                    paramsDic.Add(relatedItem.NameVarible, relatedItem.Value);
                }*/
            }
            return paramsDic;
        }

        public static string GetFormulaString(this Item item)
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
