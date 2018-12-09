using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Common.Entities;

namespace FirePlatform.Mobile.PageModels
{
    public class HomePageModel : BasePageModel
    {
        #region fields
        private readonly string _url = $"https://onedrive.live.com/download.aspx?cid=15B0AEBC04DB9C80&authKey=%21AODJTEr%5FsKI%2D5Hs&resid=15B0AEBC04DB9C80%21145&canary=Ob4rLa3XhEEl%2BLpIaWfPs40HZ6chYRmnfdoXnngg%2FBQ%3D2&ithint=%2Exml";
        private ArrayOfItemGroupSer _arrayOfItemGroup;
        private ObservableCollection<Item> _itemGroupCollection;
        #endregion fields

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
        public ArrayOfItemGroupSer ArrayOfItemGroup
        {
            get => _arrayOfItemGroup;
            set
            {
                _arrayOfItemGroup = value;
                RaisePropertyChanged(nameof(ArrayOfItemGroup));
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
                ArrayOfItemGroup = parser.Deserialize(_url);
                ItemGroupCollection = new ObservableCollection<Item>(ArrayOfItemGroup.ItemGroupSer.Items);
                IsBusy = false;
            });
        }
    }
}
