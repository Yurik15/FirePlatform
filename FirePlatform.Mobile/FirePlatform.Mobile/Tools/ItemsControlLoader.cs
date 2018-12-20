using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Common.Entities;
using FreshMvvm;

namespace FirePlatform.Mobile.Tools
{
    public class ItemsControlLoader : INotifyPropertyChanged
    {
        #region singletone
        private static ItemsControlLoader _itemsControlLoader;
        private ItemsControlLoader()
        {
        }
        public static ItemsControlLoader Intance()
        {
            if (_itemsControlLoader == null)
                _itemsControlLoader = new ItemsControlLoader();
            return _itemsControlLoader;
        }
        #endregion singletone

        #region property changed
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion property changed

        #region fields
        private readonly string _url = $"http://beforedeadline-001-site1.itempurl.com/api/Files";
        private ItemGroup[] _groups;

        private ObservableCollection<Item> _itemGroupCollection;
        #endregion fields

        public Action<string, bool> CollapseExpandGroupAction = null;

        #region bound props
        public ObservableCollection<Item> ItemGroupCollection
        {
            get => _itemGroupCollection;
            set
            {
                _itemGroupCollection = value;
                OnPropertyChanged(nameof(ItemGroupCollection));
            }
        }
        #endregion bound props

        public void Prepare(IParser<ArrayOfItemGroupSer> parser)
        {
            Task.Run(() =>
            {
                var response = parser.Deserialize(_url);
                _groups = response.ItemGroupSer;
                Refresh();
            });
        }

        public void RefreshForlumas()
        {
            ItemGroupCollection.PreparingFormula();
        }
        public void Refresh()
        {
            var result = new ObservableCollection<Item>();
            if (_groups != null)
            {
                var filteredByGroupVisibile = _groups.Where(x => x.IsVisible).ToArray();
                var unionItems = Union(filteredByGroupVisibile);
                result = new ObservableCollection<Item>(unionItems);
            }
            ItemGroupCollection = result;
            ItemGroupCollection.PreparingFormula();
        }

        private List<Item> Union(IEnumerable<ItemGroup> itemGroups)
        {
            var result = new List<Item>();
            foreach (var group in itemGroups)
            {
                result.AddRange(group.Items);
            }
            return result;
        }
        public void CollapseExpand()
        {
            if (CollapseExpandGroupAction != null)
                foreach (var group in _groups)
                {
                    CollapseExpandGroupAction(group.Title, group.Expanded);
                }
        }
    }
}
