using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Views;
using Android.Content;
using Android.Print;

namespace CuttingSystem3mkMobile.Droid
{
    [Activity(LaunchMode = LaunchMode.SingleTask, Label = "3mk Cutting system", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MvxFormsAppCompatActivity
    {
        private MyUsbReceiver usbReceiver = null;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            App.Current.InitializeNavigation();
            RegisterUsbReceiver();
        }
        private void RegisterUsbReceiver()
        {
            usbReceiver = new MyUsbReceiver();
            RegisterReceiver(usbReceiver, new IntentFilter(Android.Hardware.Usb.UsbManager.ActionUsbDeviceAttached));
            RegisterReceiver(usbReceiver, new IntentFilter(Android.Hardware.Usb.UsbManager.ActionUsbDeviceDetached));
        }
    }
}