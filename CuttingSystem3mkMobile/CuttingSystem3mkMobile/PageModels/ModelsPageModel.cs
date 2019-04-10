using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using CuttingSystem3mkMobile.Entities;
using CuttingSystem3mkMobile.Services;
using MvvmCross;
using MvvmCross.Navigation;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class ModelsPageModel : BasePageModel<DeviceDetails>
    {
        #region fields
        private DeviceDetails _deviceDetails;
        private ModelDetails[] _models;
        #endregion fields

        #region bound props

        public ModelDetails SelectedModel
        {
            get => null;
            set
            {
                if (value != null)
                {
                    SelectModelCommand?.Execute(value);
                }
            }
        }

        public ModelDetails[] Models
        {
            get => _models;
            set
            {
                _models = value;
                RaisePropertyChanged(nameof(Models));
            }
        }
        #endregion bound props

        #region CTOR
        public ModelsPageModel()
        {
            IsBackArrowVisible = true;
            PageTitle = "Models";
        }
        #endregion CTOR

        #region override
        public async override void Prepare(DeviceDetails parameter)
        {
            _deviceDetails = parameter;

            Models = await LoadModels(0, 0);

            base.Prepare(parameter);
        }
        #endregion override

        #region [Commands]
        private ICommand _selectModelCommand;
        public ICommand SelectModelCommand
        {
            get
            {
                if (_selectModelCommand == null)
                {
                    _selectModelCommand = new Command<ModelDetails>((x) => SelectModelClick(x));
                }
                return _selectModelCommand;
            }
        }

        private async void SelectModelClick(ModelDetails modelDetails)
        {
            await _mvxNavigationService.Navigate<ModelDetailsPageModel, ModelDetails>(modelDetails);
        }
        #endregion [Commands]

        #region api methods
        private async Task<ModelDetails[]> LoadModels(int customerId, int deviceId)
        {
            //Busy = true;
            var apiReturn = await _restAPI.LoadModels(customerId, deviceId);
            //Busy = false;

            if (apiReturn.DidSucceed)
            {
                return apiReturn.Entity?.Models;
            }
            else
            {
                return null;
            }
        }
        #endregion api methods
    }
}
