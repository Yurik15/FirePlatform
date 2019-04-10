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
            PageTitle = "Devices";
        }
        #endregion CTOR

        #region override
        public override Task Initialize()
        {
            Devices = LoadDevices(0).Result;
            return base.Initialize();
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
            //Busy = true;
            var apiReturn = await _restAPI.LoadDevices(customerId);
            //Busy = false;

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
