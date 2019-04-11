using System;
using Android.Content;
using CuttingSystem3mkMobile.Services;

namespace CuttingSystem3mkMobile.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class MyUsbReceiver : BroadcastReceiver, IUsbReceiverService
    {
        public static MyUsbReceiver Current;
        public MyUsbReceiver()
        {
            Current = this;
        }

        public event EventHandler<bool> OnDeviceAttach;
        public override void OnReceive(Context context, Intent intent)
        {
            bool isAttached = Android.Hardware.Usb.UsbManager.ActionUsbDeviceAttached == (intent?.Action ?? string.Empty);
            //OnDeviceAttach?.Invoke(intent, isAttached);
            string result = isAttached ? "Nawiązano połączenie z urządzeniem." : "Utracono połączenie z urządzeniem";
            App.Current.MainPage.DisplayAlert("Informacja", result, "OK");
        }
    }
}
