using System;
using System.Threading.Tasks;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Common.Entities;

namespace FirePlatform.Mobile.PageModels
{
    public class HomePageModel : BasePageModel
    {
        #region fields
        private readonly string _url = $"https://m3upfa.am.files.1drv.com/y4mcUTUblwFASp30Xqf3tHFvvVEBXwrmYGFTIUglbe6JRXaekufBVq0vMPJPYZ3Y_XM34YRtu3suWsBHbdI53gLu2N7PQtSV0iygp0q_ieD9H-CWofiv9zJ0dqKVNCnHnOkFnwZ1VAtP3Kg2m6oL0kAHEhqPPnR4541-P59dWLYN8wXjrxQVtVkNyAiLPQsCtY5/serialout%20(1).xml?download&psid=1";
        private ArrayOfItemGroupSer _arrayOfItemGroup;
        #endregion fields

        #region bound props
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
                IsBusy = false;
            });
        }
    }
}
