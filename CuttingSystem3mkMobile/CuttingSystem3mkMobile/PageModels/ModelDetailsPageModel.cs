using System;
using System.Windows.Input;
using CuttingSystem3mkMobile.Entities;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class ModelDetailsPageModel : BasePageModel<ModelDetails>
    {
        #region fields
        private ModelDetails _modelDetails;
        #endregion fields

        #region bound props
        public string Name
        {
            get => _modelDetails?.Name;
        }
        public byte[] ImagePreview
        {
            get => _modelDetails?.ImageData;
        }
        #endregion bound props

        #region CTOR
        public ModelDetailsPageModel()
        {
            IsBackArrowVisible = true;
        }
        #endregion CTOR

        #region override
        public async override void Prepare(ModelDetails parameter)
        {
            _modelDetails = parameter;
            base.Prepare(parameter);
        }
        #endregion override

        #region [Commands]
        private ICommand _startCommand;
        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new Command(() => StartClick());
                }
                return _startCommand;
            }
        }

        private async void StartClick()
        {
            await _mvxNavigationService.Navigate<QrCodeScannerPageModel, ModelDetails>(_modelDetails);
        }
        #endregion [Commands]
    }
}
