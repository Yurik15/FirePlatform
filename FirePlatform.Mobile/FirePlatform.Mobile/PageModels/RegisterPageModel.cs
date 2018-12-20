using FirePlatform.Mobile.Common.Interfaces.Communication;
using FirePlatform.Mobile.Common.Models;
using Refit;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FirePlatform.Mobile.PageModels
{
    public class RegisterPageModel : BasePageModel
    {
        #region fields

        private string _messageInfoText;

        #endregion fields

        #region bound props

        public UserCredentials CurrentUser { get; set; }

        #endregion

        public string MessageInfoText
        {
            get => _messageInfoText;
            set
            {
                _messageInfoText = value;
                RaisePropertyChanged(nameof(MessageInfoText));
            }
        }

        public RegisterPageModel()
        {
            CurrentUser = new UserCredentials();
        }

        #region Commands

        private ICommand _CreateAccountlickCommand;

        public ICommand CreateAccountClickCommand
        {
            get
            {
                if (_CreateAccountlickCommand == null)
                {
                    _CreateAccountlickCommand = new Command(() =>
                    {
                        this.CreateAccountClick();
                    });
                }
                return _CreateAccountlickCommand;
            }
        }

        private async void CreateAccountClick()
        {
            IsBusy = true;
            await Task.Run(async () =>
            {
                var isRegistered = false;

                if (CurrentUser.Password != CurrentUser.ConfirmPassword)
                {
                    MessageInfoText = "Confirm password field is wrong";
                    IsBusy = false;
                }
                else if (String.IsNullOrEmpty(CurrentUser.Login)
                || String.IsNullOrEmpty(CurrentUser.Password)
                || String.IsNullOrEmpty(CurrentUser.ConfirmPassword))
                {
                    MessageInfoText = "Fields can't be empty";
                    IsBusy = false;
                }
                else
                {
                    isRegistered = await Register(CurrentUser.Login, CurrentUser.Password);

                    if (isRegistered)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            CoreMethods.PushPageModel<LoginPageModel>();
                            IsBusy = false;
                        });
                    }
                    else
                    {
                        MessageInfoText = "Registration failed";
                        IsBusy = false;
                    }
                }
            });
        }

        #endregion

        #region Communication

        private async Task<bool> Register(string login, string password)
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
                var userCredentials = await apiResponse.Register(userRequest);
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
