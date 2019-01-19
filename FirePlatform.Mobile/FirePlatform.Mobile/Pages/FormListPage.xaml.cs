using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirePlatform.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FormListPage : ContentPage
	{
		public FormListPage ()
		{
            var shoppingCartCounterItem = new ToolbarItem();
            shoppingCartCounterItem.Icon = "shoppingCartCounter.png";
            shoppingCartCounterItem.Clicked += ShoppingCartCounterItem_Clicked;
            ToolbarItems.Add(shoppingCartCounterItem);

            InitializeComponent ();
		}

        void ShoppingCartCounterItem_Clicked(object sender, EventArgs e)
        {
        }
    }
}