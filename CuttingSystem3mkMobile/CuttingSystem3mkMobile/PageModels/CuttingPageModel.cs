using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CuttingSystem3mkMobile.Entities;
using CuttingSystem3mkMobile.Services;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class CuttingPageModel : BasePageModel<ModelDetails>
    {
        #region fields
        private ModelDetails _modelDetails;
        private readonly IPrintManager _printManager;
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
        public CuttingPageModel(IPrintManager printManager)
        {
            _printManager = printManager;
        }
        #endregion ctors

        #region override
        public async override void Prepare(ModelDetails parameter)
        {
            _modelDetails = parameter;
            base.Prepare(parameter);
        }
        #endregion override

        #region commands
        #endregion commands

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

        private void ExecuteCutCommand()
        {
            var fileData = _modelDetails.FileData;
            if (fileData != null)
            {
                Print(fileData);
            }
        }
        #endregion commands
    }
}
