﻿using System;
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
