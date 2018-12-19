using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Common.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using FirePlatform.Mobile.Models;

namespace FirePlatform.Mobile.PageModels
{
    public class HomePageModel : BasePageModel
    {
        #region fields
        private readonly string _url = $"https://m3upfa.am.files.1drv.com/y4mGk5xdimAfEoOngdCRywNwCZU6LAQ01pKSHagw07IX0UK_TRdA0Vo9TxoY_EmRZ4ywOT9Dt7KGgJkUichWVlvQ-V1A5aDrg5YdY_J0UV2HJoBtmW3UztHReaHGtlb1OaACug95B4NLW-KWPnruEIXArIFCOrl8d-6AIm8OcQjcWxkiMIZBD7KovwpUVMjb8_NOe0ZheUkhvOH3OP3taFItg/serialout%20(1).xml?download&psid=1";
        private ObservableCollection<Item> _itemGroupCollection;
        private ItemGroup[] _groups;
        private ItemGroup[] ParsedGroups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                RaisePropertyChanged(nameof(ItemGroupCollection));
            }
        }
        #endregion fields
        public Action<string, bool> CollapseExpandGroupAction = null;

        #region bound props
        public ObservableCollection<Item> ItemGroupCollection
        {
            get
            {
                if (ParsedGroups != null)
                {
                    var filteredByGroupVisibile = ParsedGroups.Where(x => x.IsVisible).ToArray();
                    var unionItems = Union(filteredByGroupVisibile);
                    var preparedFormulas = unionItems.PreparingFormula();
                    return new ObservableCollection<Item>(unionItems);
                }
                return new ObservableCollection<Item>();
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
                ParsedGroups = response.ItemGroupSer;
                IsBusy = false;
            });
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
