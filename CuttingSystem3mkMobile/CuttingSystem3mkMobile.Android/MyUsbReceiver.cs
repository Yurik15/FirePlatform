using System;
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
            App.Current.MainPage.DisplayAlert("Informacja", intent?.Action ?? string.Empty, "Ok");
        }
    }
}
