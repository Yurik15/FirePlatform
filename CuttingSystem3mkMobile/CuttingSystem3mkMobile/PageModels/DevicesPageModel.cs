using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CuttingSystem3mkMobile.Entities;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class DevicesPageModel : BasePageModel
    {
        #region fields
        private DeviceDetails[] _devicesInit;
        private Func<DeviceDetails, bool> funcFilter;
        private bool _loaded;
        private string _search;
        #endregion fields

        #region bound props
        public string SearchText
        {
            get => _search;
            set
            {
                _search = value;
                RaisePropertyChanged(nameof(Devices));
            }
        }
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
            get => _devicesInit?.Where(funcFilter).ToArray();
        }
        #endregion bound props

        #region CTOR
        public DevicesPageModel()
        {
            funcFilter = (item) =>
            {
                return string.IsNullOrEmpty((item?.Name ?? string.Empty)) ? true : item.Name.ToLower().Trim().Contains((SearchText ?? string.Empty).ToLower().Trim());
            };
        }
        #endregion CTOR

        #region override
        public async override void ViewAppeared()
        {
            if (!_loaded)
            {
                _devicesInit = await LoadDevices(0);
                _loaded = true;
            }
            await RaisePropertyChanged(nameof(Devices));
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
