using System;
using System.Collections.Generic;
using FirePlatform.Mobile.PageModels;
using Xamarin.Forms;

namespace FirePlatform.Mobile.Pages
{
    public partial class DemoTestPage : ContentPage
    {
        public DemoTestPage()
        {
            InitializeComponent();
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (BindingContext is DemoTestPageModel viewmodel)
            {
                if (Int32.TryParse(e.NewTextValue, out int value))
                {
                    viewmodel.LoadData(1, value.ToString(), value > 0 ? value : 10);
                }
            }
        }
    }
}
