using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FirePlatform.Mobile.Models;
using FirePlatform.Mobile.Tools;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace FirePlatform.Mobile.PageModels
{
    public class LoginPageModel : BasePageModel
    {
        #region fields

        private string _messageInfoText;

        #endregion fields

        #region bound props

        public UserCredentials CurrentUser { get; set; }

        public string MessageInfoText
        {
            get => _messageInfoText;
            set
            {
                _messageInfoText = value;
                RaisePropertyChanged(nameof(MessageInfoText));
            }
        }

        public bool RememberMe
        {
            get => Settings.RememberMe;
            set => Settings.RememberMe = value;
        }

        #endregion bound props

        public LoginPageModel()
        {
            if (Settings.RememberMe)
            {
                CurrentUser = new UserCredentials()
                {
                    Username = Settings.UserName,
                    Password = Settings.Password

                };
            }
            else
                CurrentUser = new UserCredentials();

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            IsInternetConnected = CrossConnectivity.Current.IsConnected;
        }

        void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            IsInternetConnected = e.IsConnected;
            if (!IsInternetConnected)
                MessageInfoText = "There is no Internet connection.";
            else
                MessageInfoText = null;
        }

        #region Commands

        private ICommand _LoginClickCommand;

        public ICommand LoginClickCommand
        {
            get
            {
                if (_LoginClickCommand == null)
                {
                    _LoginClickCommand = new Command(() =>
                    {

                        this.LoginClick();

                    });
                }
                return _LoginClickCommand;
            }
        }

        private void LoginClick()
        {
            IsBusy = true;
            Task.Run(() =>
            {
                Task.Delay(12000);
                Device.BeginInvokeOnMainThread(() =>
                {
                    CoreMethods.PushPageModel<HomePageModel>();
                    IsBusy = false;
                });
            });
        }

        #endregion Commands
    }
}
