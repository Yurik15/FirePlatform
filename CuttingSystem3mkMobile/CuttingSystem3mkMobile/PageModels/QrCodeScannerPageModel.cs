using System;
using System.Windows.Input;
using CuttingSystem3mkMobile.Entities;
using CuttingSystem3mkMobile.Services;
using MvvmCross;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class QrCodeScannerPageModel : BasePageModel<ModelDetails>
    {
        #region fields
        private ModelDetails _modelDetails;
        private readonly IQrScanningService _qrScanningService;
        private bool _hasBeenScanned;
        private string _resultQR;
        #endregion fields

        #region bound props
        public string ResultQR
        {
            get => _resultQR;
            set
            {
                _resultQR = value;
                RaisePropertyChanged(nameof(ResultQR));
            }
        }
        public bool HasBeenScanned
        {
            get => _hasBeenScanned;
            set
            {
                _hasBeenScanned = value;
                RaisePropertyChanged(nameof(HasBeenScanned));
            }
        }
        #endregion bound props

        #region ctors
        public QrCodeScannerPageModel(IQrScanningService qrScanningService)
        {
            _qrScanningService = qrScanningService;
            IsBackArrowVisible = true;
        }
        #endregion ctors

        #region override
        public override void Prepare(ModelDetails parameter)
        {
            _modelDetails = parameter;
            base.Prepare(parameter);
        }
        #endregion override

        #region commands
        private ICommand _qRScannerCommand;
        public ICommand QRScannerCommand
        {
            get
            {
                return _qRScannerCommand ?? (_qRScannerCommand = new Command(x => ExecuteQRScannerCommand()));
            }
        }

        private async void ExecuteQRScannerCommand()
        {
            if (HasBeenScanned)
            {
                await _mvxNavigationService.Navigate<CuttingPageModel, ModelDetails>(_modelDetails);
            }
            else
            {
                ResultQR = await _qrScanningService.ScanAsync();
                if (!string.IsNullOrEmpty(ResultQR))
                {
                    HasBeenScanned = true;
                }
            }
        }
        #endregion commands
    }
}
