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
        #region fields
<<<<<<< HEAD
=======
        private readonly string _url = $"https://m3upfa.am.files.1drv.com/y4mcUTUblwFASp30Xqf3tHFvvVEBXwrmYGFTIUglbe6JRXaekufBVq0vMPJPYZ3Y_XM34YRtu3suWsBHbdI53gLu2N7PQtSV0iygp0q_ieD9H-CWofiv9zJ0dqKVNCnHnOkFnwZ1VAtP3Kg2m6oL0kAHEhqPPnR4541-P59dWLYN8wXjrxQVtVkNyAiLPQsCtY5/serialout%20(1).xml?download&psid=1";
        private ArrayOfItemGroupSer _arrayOfItemGroup;
>>>>>>> develop
        #endregion fields

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
