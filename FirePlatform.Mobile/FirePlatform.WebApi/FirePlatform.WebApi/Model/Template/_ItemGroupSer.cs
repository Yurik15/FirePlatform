using System;
using System.Collections.ObjectModel;

namespace FirePlatform.WebApi.Model
{
    [Serializable]
    public class _ItemGroupSer
    {
        public bool Expanded { get; set; }
        public int NumID { get; set; }
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                }
            }
        }

        private string title = "";
        public string Tag
        {
            get { return tag; }
            set
            {
                if (tag != value)
                {
                    tag = value;
                }
            }
        }
        private string tag = "";
        public string visCondition { get; set; }

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                }
            }
        }
        private bool isVisible = true;

        public ObservableCollection<_Item> Items
        {
            get { return items; }
            set
            {
                if (items != value)
                {
                    items = value;
                }
            }
        }
        private ObservableCollection<_Item> items = new ObservableCollection<_Item>();


    }
}
