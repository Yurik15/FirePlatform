using System;
using System.Collections.Generic;
using CuttingSystem3mkMobile.PageModels;
using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.Pages
{
    [MvxContentPagePresentation(NoHistory = true)]
    public partial class DevicesPage : BaseContentPage<DevicesPageModel>
    {
        public DevicesPage()
        {
            InitializeComponent();
        }
    }
}
