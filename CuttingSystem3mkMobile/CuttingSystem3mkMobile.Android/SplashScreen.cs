using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Views;

namespace CuttingSystem3mkMobile.Droid
{
    [Activity(
        Label = "3mk Cutting system"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/SplashTheme"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxFormsSplashScreenActivity<Setup, CoreApp, CuttingSystem3mkMobile.App>
    {
        public SplashScreen()
            : base()
        {
        }

        protected override async Task RunAppStartAsync(Bundle bundle)
        {
            StartActivity(typeof(MainActivity));
            await base.RunAppStartAsync(bundle);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }
    }
}
