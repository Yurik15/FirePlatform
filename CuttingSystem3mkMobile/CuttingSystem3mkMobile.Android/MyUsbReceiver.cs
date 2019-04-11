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
        }
    }
}
