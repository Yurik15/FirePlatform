using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using FirePlatform.Mobile.Droid.PushNotification;

namespace FirePlatform.Mobile.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {

        }
        public override void OnCreate()
        {
            base.OnCreate();
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                PushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                //Change for your default notification channel name here
                PushNotificationManager.DefaultNotificationChannelName = "General";
            }
#if DEBUG
            PushNotificationManager.Initialize(this, true, pushNotification: new Lazy<Common.PushNotification.Abstractions.IPushNotification>(() => { return new PushNotificationManager(); }));
#else
              PushNotificationManager.Initialize(this,false, pushNotification: new Lazy<Common.PushNotification.Abstractions.IPushNotification>(() => { return new PushNotificationManager(); }));
#endif

        }
    }
}
