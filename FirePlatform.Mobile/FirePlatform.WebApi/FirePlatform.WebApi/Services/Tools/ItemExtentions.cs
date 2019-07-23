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
                if (relatedGroup.Title.ToLower().Equals("spacing requirements"))
                {

                }
                relatedGroup.UpdateGroup();
            }
            foreach (var relatedItem in item.NeedNotifyItems)
            {
                if(relatedItem.Title.Trim().ToLower().Contains("ceiling slope"))
                {

                }
                if (relatedItem.ParentGroup.IsVisible) // PERFORMANCE
                    relatedItem.UpdateItem();
            }
        }
        public static void UpdateItem(this Item item)
        {
            if (item.ParentGroup.IsVisible)
            {
                //item.IsVisiblePrev = item.IsVisible;
                var formula = item.Formula;
                var visCondition = item.VisCondition;

                if (!string.IsNullOrEmpty(visCondition))
                {
                    var paramsDic = ItemExtentions.GetParams(item.DependToItems);
                    var res = CalculationTools.CalculateVis(item.VisCondition, paramsDic);
                    item.IsVisible = res.HasValue ? res.Value : false;
                }
                else
                {
                    item.IsVisible = true;
                }
                if (!string.IsNullOrEmpty(formula) && item.IsVisible)
                {
                    var paramsDic = ItemExtentions.GetParams(item.DependToItemsForFormulas);
                    var res = CalculationTools.CalculateFormulas(formula, paramsDic);
                    item.Value = res;
                    //item?.NotifyAboutChange();
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

        public static Dictionary<string, object> GetParams(List<KeyValuePair<string, List<DataDependItem>>> dependToItems)
        {
            Dictionary<string, object> paramsDic = new Dictionary<string, object>();
            List<DataDependItem> depends = new List<DataDependItem>();
            foreach (var item in dependToItems)
            {
                try
                {
                    var visibleItems = item.Value.Where(x => x.ReferencedItem.IsVisible).ToList();
                    if (visibleItems.Any())
                    {
                        depends.Add(visibleItems.FirstOrDefault());
                    }
                    else
                    {
                        if (item.Key.Contains(","))
                        {
                            var parts = item.Key.Split(",");
                            foreach (var part in parts)
                            {
                                if (!paramsDic.ContainsKey(part))
                                {
                                    paramsDic.Add(part, null);
                                }
                            }
                        }
                        else
                        {
                            if (!paramsDic.ContainsKey(item.Key))
                            {
                                paramsDic.Add(item.Key, null);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            var param = GetParams(depends);
            foreach (var pr in param)
            {
                if (!paramsDic.ContainsKey(pr.Key))
                {
                    paramsDic.Add(pr.Key, pr.Value);
                }
                else
                {
                    paramsDic[pr.Key] = pr.Value;
                }
            }
            return paramsDic;
        }
        public static Dictionary<string, object> GetParams(List<DataDependItem> dependToItems)
        {
            var paramsDic = new Dictionary<string, object>();
            foreach (var relatedItem in dependToItems)
            {
                var paramsFromItem = GetParam(relatedItem);
                foreach (var param in paramsFromItem)
                {
                    if (paramsDic.ContainsKey(param.Key))
                    {

                    }
                    else
                    {
                        paramsDic.Add(param.Key, param.Value);
                    }
                }
            }
            return paramsDic;
        }

        private static Dictionary<string, object> GetParam(DataDependItem relatedItem)
        {
            var paramsDic = new Dictionary<string, object>();

            if (relatedItem.ReferencedItem.Type == ItemType.Combo.ToString())
            {
                var name = relatedItem.Name;
                bool? value = false;
                if (paramsDic.ContainsKey(name.Trim()))
                {

                }
                else
                {
                    var selectedValue = relatedItem.ReferencedItem.NameVarible;
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
                                value = selectedValue.Contains(",") ? selectedValue.Split(",").Select(x=>x.Trim()).ToList().Contains(nm) : selectedValue == nm;
                                value = relatedItem.ReferencedItem.IsVisible ? value : null;
                                paramsDic.Add(nm.Trim(), value);
                            }
                        }
                    }
                    else
                    {
                        value = selectedValue == name;
                    }
                    if (!name.Contains(","))
                    {
                        value = relatedItem.ReferencedItem.IsVisible ? value : null;
                        paramsDic.Add(name.Trim(), value);
                    }
                }
            }
            else if (relatedItem.ReferencedItem.Type == ItemType.Text.ToString())
            {
                if (paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible.Trim()))
                {

                }
                else
                {
                    var value = relatedItem.ReferencedItem.IsVisible ? relatedItem.ReferencedItem.Value ?? string.Empty : null;
                    paramsDic.Add(relatedItem.ReferencedItem.NameVarible.Trim(), value);
                }
            }
            else if (relatedItem.ReferencedItem.Type == ItemType.Num.ToString())
            {
                if (paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible.Trim()))
                {

                }
                else
                {
                    var value = relatedItem.ReferencedItem.IsVisible ? (relatedItem.ReferencedItem.Value ?? -9999999) : null;
                    paramsDic.Add(relatedItem.ReferencedItem.NameVarible.Trim(), value);
                }
            }
            else if (relatedItem.ReferencedItem.Type == ItemType.Check.ToString())
            {
                if (paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible.Trim()))
                {

                }
                else
                {
                    var value = relatedItem.ReferencedItem.IsVisible ? (relatedItem.ReferencedItem.Value ?? false) : null;
                    paramsDic.Add(relatedItem.ReferencedItem.NameVarible.Trim(), value);
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
                    var value = relatedItem.ReferencedItem.IsVisible ? (param.value ?? false) : null;
                    paramsDic.Add(param.name.Trim(), value);
                }
            }
            else if (relatedItem.ReferencedItem.Type == ItemType.Hidden.ToString())
            {
                if (paramsDic.ContainsKey(relatedItem.ReferencedItem.NameVarible.Trim()))
                {

                }
                else
                {
                    var param = GetParamsFromFormulasAfterCalculation(relatedItem);
                    var name = param.name.Trim();
                    var value = param.value;
                    if (!relatedItem.ReferencedItem.IsVisible)
                    {
                        value = ValueIfGFormulaIsNotVisible(value);
                    }
                    value = relatedItem.ReferencedItem.IsVisible ? value : null;
                    paramsDic.Add(name, value);
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
                        var value = relatedItem.ReferencedItem.IsVisible ? element.Value : null;
                        paramsDic.Add(element.Key.Trim(), value);
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
                    var nameVarible = relatedItem.Key;
                    if (!string.IsNullOrEmpty(nameVarible) && !paramsDic.ContainsKey(nameVarible))
                    {
                        var visRelatedItem = relatedItem.Value.Where(x => x.ReferencedItem.IsVisible).ToArray();
                        object value = null;
                        if (visRelatedItem.Any())
                        {
                            value = visRelatedItem.First().ReferencedItem.Value;
                        }
                        else
                        {
                            value = relatedItem.Value.First().ReferencedItem.Value;
                        }

                        if (string.IsNullOrEmpty(nameVarible))
                        {
                            nameVarible = relatedItem.Key;
                        }
                        if (value == null || string.IsNullOrEmpty(value as string))
                        {
                            var type = relatedItem.Value.First().ReferencedItem.Type;
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
                var name = ghostFormula.Name;
                object value = null;
                if (ghostFormula.DependToItems.Any())
                {
                    var paramsGhostFormula = ghostFormula.GetParams();
                    value = CalculationTools.CalculateFormulas(ghostFormula.Conditions, paramsGhostFormula); //TODO need to check if element is visible but the return type is unknown and we need set the element any value (if not set then app throw exception)
                }
                else if (!paramsDic.ContainsKey(ghostFormula.Name))
                {
                    value = ghostFormula.Conditions;
                }
                if (!itemIsVisible)
                {
                    value = ValueIfGFormulaIsNotVisible(value);
                }
                paramsDic.Add(name, value);
            }
            return paramsDic;
        }

        private static object ValueIfGFormulaIsNotVisible(object value)
        {
            string newValue = value?.ToString();
            if (bool.TryParse(newValue, out bool resultBool))
            {
                return false;
            }
            if (double.TryParse(newValue, out double resultDouble))
            {
                return 0;
            }
            return value;
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
