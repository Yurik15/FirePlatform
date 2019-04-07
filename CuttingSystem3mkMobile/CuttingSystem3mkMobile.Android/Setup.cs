using System;
using Android.Print;
using CuttingSystem3mkMobile.Services;
using Java.Util.Logging;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Logging;
using MvvmCross.ViewModels;

namespace CuttingSystem3mkMobile.Droid
{
    public class Setup : MvxFormsAndroidSetup<CoreApp, App>
    {
        protected override Xamarin.Forms.Application CreateFormsApplication()
        {
            Mvx.IoCProvider.RegisterSingleton<IPrintManager>(new CuttingSystem3mkMobile.Droid.Implementations.PrintManager());
            Mvx.IoCProvider.RegisterSingleton<IQrScanningService>(new CuttingSystem3mkMobile.Droid.Implementations.QrScanningService());

            return new CuttingSystem3mkMobile.App();
        }

        protected override IMvxApplication CreateApp()
        {
            return new CoreApp();
        }

        protected override IMvxFormsPagePresenter CreateFormsPagePresenter(IMvxFormsViewPresenter viewPresenter)
        {
            var formsPresenter = base.CreateFormsPagePresenter(viewPresenter);
            Mvx.IoCProvider.RegisterSingleton(formsPresenter);

            return formsPresenter;
        }
    }
}
