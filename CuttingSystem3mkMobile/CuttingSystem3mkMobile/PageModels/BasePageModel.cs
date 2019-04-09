using System;
using CuttingSystem3mkMobile.RestAPI;
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
        protected readonly IRemoteService _restAPI;
        #endregion fields

        public bool Busy
        {
            get; set;
        }

        protected BasePageModel()
        {
            if (Mvx.IoCProvider.CanResolve<IMvxNavigationService>())
            {
                _mvxNavigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
                _restAPI = Mvx.IoCProvider.Resolve<IRemoteService>();
            }
        }
    }
}
