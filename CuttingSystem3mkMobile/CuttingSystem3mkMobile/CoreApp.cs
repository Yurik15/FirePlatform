using System;
using CuttingSystem3mkMobile.PageModels;
using MvvmCross.ViewModels;

namespace CuttingSystem3mkMobile
{
    public class CoreApp : MvxApplication
    {
        public override void Initialize()
        {

            // Android - CurrentPageModelType has not yet been set, but we can default to LoginPageModel and
            // allow Precision.Mobile.App to set the correct start page after Initialize has been called.
            /*if (App.Current.CurrentPageModelType == null)
            {
                RegisterAppStart<LoginPageModel>();
            }
            // iOS - Precision.Mobile.App has already set the start page, and also its matching PageModel.
            else
            {
                if (App.Current.CurrentPageModelType == typeof(PinPageModel))
                {
                    RegisterAppStart<PinPageModel>();
                }
                else if (App.Current.CurrentPageModelType == typeof(LoginPageModel))
                {
                    RegisterAppStart<LoginPageModel>();
                }
            }*/
            RegisterAppStart<ModelsPageModel>();
        }
    }
}
