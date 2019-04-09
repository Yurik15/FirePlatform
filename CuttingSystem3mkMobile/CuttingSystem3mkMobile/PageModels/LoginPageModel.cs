using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class LoginPageModel : BasePageModel
    {
        #region Private Fields
        private string _password;
        private string _userName;
        private string _errorMessage;

        #endregion Private Fields

        #region bound props
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public bool IsLoginButtonEnabled
        {
            get => AreCredentialsInserted();
            set
            {
                RaisePropertyChanged(nameof(IsLoginButtonEnabled));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }

        #endregion bound props

        #region Public Constructors
        public LoginPageModel()
        {
        }
        #endregion Public Constructors

        #region commands
        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new Command(x => ExecuteLoginCommand()));
            }
        }
        protected async void ExecuteLoginCommand()
        {
            await _mvxNavigationService.Navigate<DevicesPageModel>();
        }
        private ICommand _goToSignUpCommand;
        public ICommand GoToSignUpCommand
        {
            get
            {
                return _goToSignUpCommand ?? (_goToSignUpCommand = new Command(x => ExecuteGoToSignUpCommand()));
            }
        }
        protected async void ExecuteGoToSignUpCommand()
        {
        }
        #endregion commands

        #region helper methods
        private bool AreCredentialsInserted()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }
        private void ResetPassword()
        {
            Password = string.Empty;
        }

        private void ResetErrorMessage()
        {
            ErrorMessage = string.Empty;
        }
        #endregion helper methods
    }
}
