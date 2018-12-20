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
using FirePlatform.Mobile.Tools;

namespace FirePlatform.Mobile.PageModels
{
    public class HomePageModel : BasePageModel
    {
        #region bound properties
        public ItemsControlLoader ItemsControlLoader { get; set; }

        #endregion bound properties

        public HomePageModel(IParser<ArrayOfItemGroupSer> parser)
        {
            ItemsControlLoader = ItemsControlLoader.Intance();
            Prepare(ItemsControlLoader, parser);
        }

        public void Prepare(ItemsControlLoader itemsControlLoader, IParser<ArrayOfItemGroupSer> parser)
        {
            IsBusy = true;
            Task.Run(() =>
            {
                itemsControlLoader.Prepare(parser);
                IsBusy = false;
            });
        }
    }
}
