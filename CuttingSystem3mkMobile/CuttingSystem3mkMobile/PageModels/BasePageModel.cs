using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CuttingSystem3mkMobile.RestAPI;
using CuttingSystem3mkMobile.Services;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public abstract class BasePageModel<T> : BasePageModel, IMvxViewModel<T>
    {
        public virtual void Prepare(T parameter) { }
    }
    public abstract class BasePageModel : MvxViewModel
    {
        #region fields
        private bool _busy;
        protected readonly IMvxNavigationService _mvxNavigationService;
        protected readonly IRemoteService _restAPI;
        #endregion fields

        public string PageTitle
        {
            get; set;
        } = "3mk Cutting system";
        public bool Busy
        {
            get => _busy;
            set
            {
                _busy = value;
                RaisePropertyChanged(nameof(Busy));
            }
        }
        public bool DeviceConnected
        {
            get => ApplicationContext.ApplicationContext.DeviceConnected;
        }
        public bool IsBackArrowVisible
        {
            get; set;
        }

        protected BasePageModel()
        {
            if (Mvx.IoCProvider.CanResolve<IMvxNavigationService>())
            {
                _mvxNavigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            }
            _restAPI = Mvx.IoCProvider.Resolve<IRemoteService>();
        }

        public override Task Initialize()
        {
            ApplicationContext.ApplicationContext.OnDeviceAttach += UsbReceiverService_OnDeviceAttach;
            return base.Initialize();
        }
        void UsbReceiverService_OnDeviceAttach(object sender, bool e)
        {
            RaisePropertyChanged(nameof(DeviceConnected));
        }

        #region commands
        private ICommand _backCommand;
        public ICommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new Command(async () => await ExecuteBackCommand()));
            }
        }

        protected virtual async Task ExecuteBackCommand()
        {
            await _mvxNavigationService.Close(this);
        }
        #endregion commands
    }
}
