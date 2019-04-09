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
        public override Task Initialize()
        {
            Devices = LoadDevices(0).Result;
            return base.Initialize();
        }
        public async override void ViewAppearing()
        {
            //;
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

        private void SelectDeviceClick(DeviceDetails deviceDetails)
        {

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
