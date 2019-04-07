using CuttingSystem.Mobile.Common.Entities;
using CuttingSystem.Mobile.Common.Entities.Container;
using CuttingSystem.Mobile.Common.Interfaces.Communication;
using Refit;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.PageModels
{
    public class DeviceListPageModel : BasePageModel
    {
        #region fields

        private DeviceModel _selectedDeviceModel;

        #endregion

        #region prop
        private ObservableCollection<DeviceModel> _DeviceModels;
        public ObservableCollection<DeviceModel> DeviceModels
        {
            get => _DeviceModels;
            set
            {
                _DeviceModels = value;
                RaisePropertyChanged(nameof(_DeviceModels));
            }
        }
        public DeviceModel SelectedDevice
        {
            set
            {
                _selectedDeviceModel = value;
            }
        }

        #endregion

        #region ctor

        public DeviceListPageModel()
        {
            LoadData();
        }

        #endregion


        #region Navigation

        private void LoadData()
        {
            try
            {
                var apiResponse = RestService.For<ICutApi>(RestApiServerUri);
                ApiContainer<DeviceModel> container = null;
                Task.Run(async () =>
                {
                    container = await apiResponse.GetDeviceModels();
                    DeviceModels = new ObservableCollection<DeviceModel>(container?.DataCollection);
                });
            }
            catch (Exception ex)
            {

            }

            #endregion
        }
    }
}
