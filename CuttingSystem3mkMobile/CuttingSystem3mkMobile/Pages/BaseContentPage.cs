using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;

namespace CuttingSystem3mkMobile.Pages
{
    [MvxContentPagePresentation(Animated = true)]
    public class BaseContentPage<TViewModel> : MvxContentPage<TViewModel> where TViewModel : class, MvvmCross.ViewModels.IMvxViewModel
    {
        public BaseContentPage()
        {
        }
    }
}
