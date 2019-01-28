using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace FirePlatform.Mobile.Common.Entities
{
    public class Item : INotifyPropertyChanged
    {
        public string[] GhostFormulas { get; set; }
        public string[] MultiItemTags { get; set; }
        public string[] MultiItemTitles { get; set; }
        public MyComboItem[] MultiItemDict { get; set; }
        public string Type { get; set; }
        public string Formula { get; set; }
        [XmlElement(ElementName = "visCondition")]
        public string VisCondition { get; set; }
        public string Title { get; set; }
        public string NumID { get; set; }
        public string TooltipText { get; set; }
        public string GroupTitle { get; set; }
        public string GroupNum { get; set; }
        public bool IsChecked { get; set; }
        public bool SuspendPropertyChanged { get; set; }
        public string SelectedIndex { get; set; }
        public string NumVar { get; set; }
        public double NumValue { get; set; }
        //public virtual string NumValueString { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Inc { get; set; }
        public string Dec { get; set; }
        public string NumVariable { get; set; }
        public bool IsVisible { get; set; }
        public bool WasVisible { get; set; }
        public bool IsGroupVisible { get; set; }
        public bool IsVisibleCombo { get; set; }
        public bool IsVisibleNum { get; set; }
        public bool IsVisibleCheck { get; set; }
        public bool IsVisibleText { get; set; }

        #region props
        public string NumValueString
        {
            get
            {
                return NumValue.ToString();
            }
            set
            {
                if (double.TryParse(value, out double parsedValue))
                    NumValue = parsedValue;
                else
                    NumValue = default(double);
                OnPropertyChanged(nameof(NumValueString));
            }
        }
        #endregion props

        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion property changed
        #region ignor serialize&deserialize

        public static string[] Operations = { "+", "-", "*", "/" };
        private ObservableCollection<Item> _parametersDependToItem;
        public List<Item> ItemsToNeedNotify { get; set; }
        public ObservableCollection<Item> ParametersDependToItem
        {
            get => _parametersDependToItem;
            set
            {
                _parametersDependToItem = new ObservableCollection<Item>();
                if (value.Any())
                {
                    if (!string.IsNullOrEmpty(Formula))
                    {
                        var formula = FormulaHelper.GetFormulaString(Formula, out _);
                        var variblesFromFormula = formula.Split(Operations, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var varible in variblesFromFormula)
                        {
                            var item = value.FirstOrDefault(x => x.NumVar == varible);
                            if (item == null)
                            {
                                item = value.FirstOrDefault(x => x.Formula?.Split('=')[0].Trim() == varible);
                            }
                            if (item != null && !_parametersDependToItem.Contains(item))
                            {
                                if (string.IsNullOrEmpty(item.NumVar))
                                    item.NumVar = varible;
                                item.ItemsToNeedNotify.Add(this);
                                _parametersDependToItem.Add(item);
                            }
                        }
                    }
                }
            }
        }
        public void Update()
        {
            if (!string.IsNullOrEmpty(Formula))
            {
                if (_parametersDependToItem != null)
                {
                    var a = _parametersDependToItem
                                            .Where(x => !string.IsNullOrEmpty(x.NumVar))
                                            .Select(x => new
                                            {
                                                key = x.NumVar,
                                                value = x.NumValue
                                            }).ToList();
                    var varibleDitionary = a
                                            .ToDictionary(x => x.key, y => (object)y.value);

                    var formula = FormulaHelper.GetFormulaString(Formula, out _);
                    NumValueString = FormulaHelper.Calculate(formula, varibleDitionary).ToString();
                }
            }
        }
        #endregion ignor serialize&deserialize
    }
}
