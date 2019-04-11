using Android.Content;

namespace CuttingSystem3mkMobile.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class MyUsbReceiver : BroadcastReceiver
    {

        public MyUsbReceiver()
        {

        }
        public override void OnReceive(Context context, Intent intent)
        {
            bool isAttached = Android.Hardware.Usb.UsbManager.ActionUsbDeviceAttached == (intent?.Action ?? string.Empty);
            ApplicationContext.ApplicationContext.DeviceConnected = isAttached;
            string result = isAttached ? "Nawiązano połączenie z urządzeniem." : "Utracono połączenie z urządzeniem";
            App.Current.MainPage.DisplayAlert("Informacja", result, "OK");
        }
    }
}
