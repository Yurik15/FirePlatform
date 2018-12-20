using System;
using FreshMvvm;

namespace FirePlatform.Mobile.PageModels
{
    public class BasePageModel : FreshBasePageModel
    {
        #region fields

        private bool _isBusy;
        private bool _isInternetConnected;

        protected const string RestApiServerUri = "http://beforedeadline-001-site1.itempurl.com/";
        //protected const string RestApiServerUri = "https://localhost:44358/";    

        #endregion fields

        #region bound props

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }
        public bool IsInternetConnected
        {
            get => _isInternetConnected;
            set
            {
                _isInternetConnected = value;
                RaisePropertyChanged(nameof(IsInternetConnected));
            }
        }
        #endregion bound props
    }
}
