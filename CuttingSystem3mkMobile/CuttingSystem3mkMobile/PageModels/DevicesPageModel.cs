using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CuttingSystem3mkMobile.Entities;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class DevicesPageModel : BasePageModel
    {
        #region fields
        private DeviceDetails[] _devices;
        bool _loaded;
        #endregion fields

        #region bound props

        public DeviceDetails SelectedDevice
        {
            get => null;
            set
            {
                if (value != null)
                {
                    SelectDeviceCommand?.Execute(value);
                }
            }
        }

        public DeviceDetails[] Devices
        {
            get => _devices;
            set
            {
                _devices = value;
                RaisePropertyChanged(nameof(Devices));
            }
        }
        #endregion bound props

        #region CTOR
        public DevicesPageModel()
        {
        }
        #endregion CTOR

        #region override
        public async override void ViewAppeared()
        {
            if (!_loaded)
            {
                Devices = await LoadDevices(0);
                _loaded = true;
            }
            await RaisePropertyChanged(nameof(SelectedDevice));
            base.ViewAppearing();
        }
        #endregion override

        #region [Commands]
        private ICommand _selectDeviceCommand;
        public ICommand SelectDeviceCommand
        {
            get
            {
                if (_selectDeviceCommand == null)
                {
                    _selectDeviceCommand = new Command<DeviceDetails>((x) => SelectDeviceClick(x));
                }
                return _selectDeviceCommand;
            }
        }

        private async void SelectDeviceClick(DeviceDetails deviceDetails)
        {
            await _mvxNavigationService.Navigate<ModelsPageModel, DeviceDetails>(deviceDetails);
        }
        #endregion [Commands]

        #region api methods
        private async Task<DeviceDetails[]> LoadDevices(int customerId)
        {
            Busy = true;
            await Task.Delay(3000);
            var apiReturn = await _restAPI.LoadDevices(customerId);
            Busy = false;
            if (apiReturn.DidSucceed)
            {
                return apiReturn.Entity?.Devices;
            }
            else
            {
                return null;
            }
        }
        #endregion api methods
    }
}
