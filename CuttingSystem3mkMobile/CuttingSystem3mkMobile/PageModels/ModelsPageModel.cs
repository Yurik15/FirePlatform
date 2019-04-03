using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using CuttingSystem3mkMobile.Services;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.PageModels
{
    public class ModelsPageModel : BasePageModel
    {
        #region fields
        private readonly IPrintManager _printManager;
        private string infoMsg;
        #endregion fields

        #region ctors
        public ModelsPageModel(IPrintManager printManager)
        {
            _printManager = printManager;
        }
        #endregion ctors

        #region bound props
        public string InfoMsg
        {
            get => infoMsg;
            set
            {
                infoMsg = value;
                RaisePropertyChanged();
            }
        }
        #endregion bound props

        #region commands
        private ICommand _demoCommand;
        public ICommand DemoCommand
        {
            get
            {
                return _demoCommand ?? (_demoCommand = new Command(x => ExecuteDemoCommand()));
            }
        }

        private void ExecuteDemoCommand()
        {
            var bytes = FakeDocument();
            var devices = _printManager.Devices();
            if (devices.Any())
            {
                var device = devices.First();
                _printManager.ConnectAndSend(bytes, device.ProductId, device.VendorId);
            }
            else
            {
                InfoMsg = "Not found devices.";
            }
        }
        #endregion commands

        #region helper methods
        private byte[] FakeDocument()
        {
            Assembly currentAssembly = typeof(ModelsPageModel).GetTypeInfo().Assembly;
            byte[] buffer = null;
            using (var resourceStream = currentAssembly.GetManifestResourceStream("CuttingSystem3mkMobile.asus-zenfone-2-delux.plt"))
            {
                buffer = new byte[resourceStream.Length];
                resourceStream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
        #endregion helper methods
    }
}
