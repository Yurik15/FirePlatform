using CuttingSystem3mkMobile.PageModels;
using CuttingSystem3mkMobile.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CuttingSystem3mk.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceListPage : BaseContentPage<DeviceListPageModel>
    {
        public DeviceListPage()
        {
            InitializeComponent();
        }
    }
}