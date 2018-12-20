using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FirePlatform.Mobile.Common.Interfaces.Communication;
using FirePlatform.Mobile.Common.Models;
using FirePlatform.Mobile.Tools;
using Plugin.Connectivity;
using Refit;
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
                    Login = Settings.Login,
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

        private async void LoginClick()
        {
            IsBusy = true;
            await Task.Run(async() =>
            {
                var isAuthenticated = false;
                isAuthenticated = await UserAuthenticated(CurrentUser.Login, CurrentUser.Password);

                if (isAuthenticated)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CoreMethods.PushPageModel<HomePageModel>();
                        IsBusy = false;
                    });
                }
                else
                {
                    MessageInfoText = "Login failed";
                    IsBusy = false;
                }
            });
        }

        private ICommand _RegisterlickCommand;

        public ICommand RegisterClickCommand
        {
            get
            {
                if (_RegisterlickCommand == null)
                {
                    _RegisterlickCommand = new Command(() =>
                    {
                        this.RegisterClick();
                    });
                }
                return _RegisterlickCommand;
            }
        }

        private void RegisterClick()
        {
            CoreMethods.PushPageModel<RegisterPageModel>();
        }

        #endregion Commands

        #region Communication

        private async Task<bool> UserAuthenticated(string login, string password)
        {
            if (string.IsNullOrEmpty(login)
                || string.IsNullOrEmpty(password))
            {
                return false;
            }
            var apiResponse = RestService.For<IAccountApi>(RestApiServerUri);
            var userRequest = new UserCredentials()
            {
                Login = login,
                Password = password
            };
            try
            {
                var userCredentials = await apiResponse.Login(userRequest);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
