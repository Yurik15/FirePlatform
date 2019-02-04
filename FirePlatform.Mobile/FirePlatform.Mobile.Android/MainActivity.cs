using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FirePlatform.Mobile.Droid.PushNotification;
using Android.Content;

namespace FirePlatform.Mobile.Droid
{
    [Activity(LaunchMode = LaunchMode.SingleTask, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            PushNotificationManager.ProcessIntent(this, Intent);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            PushNotificationManager.ProcessIntent(this, intent);
        }
    }
}