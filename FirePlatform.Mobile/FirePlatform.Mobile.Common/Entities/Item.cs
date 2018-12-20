using System;
using System.ComponentModel;
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
        public string NumValueString
        {
            get => NumValue.ToString();
            set
            {
                if (double.TryParse(value, out double parsedValue))
                    NumValue = parsedValue;
                else
                    NumValue = default(double);
                OnPropertyChanged(nameof(NumValueString));
            }
        }
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

        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion property changed

    }
}
