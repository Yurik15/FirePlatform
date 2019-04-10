using System;
using System.Threading.Tasks;
using CuttingSystem3mkMobile.Services;
using ZXing.Mobile;

namespace CuttingSystem3mkMobile.Droid.Implementations
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Zeskanuj kod QR na folii",
                BottomText = "Proszę czekać",
            };

            var scanResult = await scanner.Scan(Android.App.Application.Context, optionsCustom);
            return scanResult.Text;
        }
    }
}
