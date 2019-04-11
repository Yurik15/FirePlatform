using CuttingSystem3mkMobile.Entities;
using CuttingSystem3mkMobile.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class CuttingPageModel : BasePageModel<(string qrCode, ModelDetails modelDetails)>
    {
        #region fields
        private string _qrCode;
        private ModelDetails _modelDetails;
        private readonly IPrintManager _printManager;
        private string _infoMsg;
        private bool _isCut;
        private bool _isCorrect;
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
        public bool IsCut
        {
            get => _isCut;
            set
            {
                _isCut = value;
                RaisePropertyChanged(nameof(IsCut));
            }
        }
        public bool IsCorrect
        {
            get => _isCorrect;
            set
            {
                _isCorrect = value;
                RaisePropertyChanged(nameof(IsCorrect));
            }
        }
        #endregion bound props

        #region ctors
        public CuttingPageModel(IPrintManager printManager)
        {
            _printManager = printManager;
        }
        #endregion ctors

        #region override
        public override void Prepare((string qrCode, ModelDetails modelDetails) parameter)
        {
            _modelDetails = parameter.modelDetails;
            _qrCode = parameter.qrCode;
            base.Prepare(parameter);
        }
        #endregion override

        #region helper method
        private async void Print(byte[] data)
        {
            try
            {
                await Task.Run(async () =>
                {
                    Busy = true;
                    var devices = _printManager.Devices();
                    if (devices != null && devices.Any())
                    {
                        var device = devices.First();
                        _printManager.ConnectAndSend(data, device.ProductId, device.VendorId);
                        IsCut = ApplicationContext.ApplicationContext.PermissionUSB;
                    }
                    else
                    {
                        InfoMsg = "Nie znaleziono urządzenia.";
                    }
                    await Task.Delay(500);
                    Busy = false;
                });
            }
            catch (Exception ex)
            {
                InfoMsg = ex.Message;
            }
        }
        #endregion helper method

        #region commands
        private ICommand _cutCommand;
        public ICommand CutCommand
        {
            get
            {
                return _cutCommand ?? (_cutCommand = new Command(x => ExecuteCutCommand()));
            }
        }

        private async void ExecuteCutCommand()
        {
            if (IsCut)
            {
                if (IsCorrect)
                {
                     await SetDisabledCode(_qrCode);
                }
                await _mvxNavigationService.Navigate<DevicesPageModel>();
            }
            else
            {
                var fileData = _modelDetails.FileData;
                if (fileData != null)
                {
                    Print(fileData);
                }
            }
        }
        #endregion commands

        #region api methods
        private async Task SetDisabledCode(string code)
        {
            Busy = true;
            await _restAPI.SetDisabledCode(code);
            Busy = false;
        }
        #endregion api methods
    }
}
