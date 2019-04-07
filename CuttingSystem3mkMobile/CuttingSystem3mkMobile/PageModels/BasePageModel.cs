using System;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CuttingSystem3mkMobile.PageModels
{
    public abstract class BasePageModel<T> : BasePageModel, IMvxViewModel<T>
    {
        public virtual void Prepare(T parameter) { }
    }
    public abstract class BasePageModel : MvxViewModel
    {
        #region fields
        protected readonly IMvxNavigationService _mvxNavigationService;
        #endregion fields
        protected BasePageModel()
        {
            if (Mvx.IoCProvider.CanResolve<IMvxNavigationService>())
            {
                _mvxNavigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
            }
        }
    }
}
