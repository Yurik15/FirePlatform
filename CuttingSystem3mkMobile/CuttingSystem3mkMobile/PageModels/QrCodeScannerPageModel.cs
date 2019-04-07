using System;
using System.Windows.Input;
using CuttingSystem3mkMobile.Services;
using MvvmCross;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class QrCodeScannerPageModel : BasePageModel
    {
        #region fields
        private readonly IQrScanningService _qrScanningService;
        private string _infoMsg;
        #endregion fields

        #region bound props
        public string InfoMsg
        {
            get => _infoMsg;
            set
            {
                _infoMsg = value;
                RaisePropertyChanged();
            }
        }
        #endregion bound props

        #region ctors
        public QrCodeScannerPageModel(IQrScanningService qrScanningService)
        {
            _qrScanningService = qrScanningService;
        }
        #endregion ctors

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
            InfoMsg = await _qrScanningService.ScanAsync();
        }
        #endregion commands
    }
}
