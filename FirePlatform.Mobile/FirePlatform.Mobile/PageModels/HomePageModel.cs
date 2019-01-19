using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Common.Entities;
using FirePlatform.Mobile.Tools;

namespace FirePlatform.Mobile.PageModels
{
    public class HomePageModel : BasePageModel
    {
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
                RaisePropertyChanged(nameof(ItemGroupCollection));
            }
        }
        #endregion bound props

        public HomePageModel(IParser<ArrayOfItemGroupSer> parser)
        {

            Prepare(parser);
        }

        public void Prepare(IParser<ArrayOfItemGroupSer> parser)
        {
            IsBusy = true;
            Task.Run(() =>
            {
                var response = parser.Deserialize(_url);
                _groups = response.ItemGroupSer;
                Refresh();
                IsBusy = false;
            });
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
            Calculate();
            //ItemGroupCollection.PreparingFormula();
        }

        private void Calculate()
        {
            foreach (var item in ItemGroupCollection)
            {
                item.Update();
            }
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
