using System;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace FirePlatform.Mobile.Droid
{
    [Activity(Label = "Fire Platform", Icon = "@drawable/logo", Theme = "@style/SplashTheme", NoHistory = true, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
            Finish();
            OverridePendingTransition(0, 0);
        }
    }
}
