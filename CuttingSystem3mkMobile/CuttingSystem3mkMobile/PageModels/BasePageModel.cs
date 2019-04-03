using System;
using MvvmCross.ViewModels;

namespace CuttingSystem3mkMobile.PageModels
{
    public abstract class BasePageModel<T> : BasePageModel, IMvxViewModel<T>
    {
        public BasePageModel()
        {
        }
        public virtual void Prepare(T parameter) { }
    }
    public abstract class BasePageModel : MvxViewModel
    {
    }
}
