using System;
using CuttingSystem3mkMobile.Pages;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile
{
    public partial class App : Application
    {
        public static new App Current;
        public App()
        {
            Current = this;
            InitializeComponent();
            RegisterEventHandlers();
            Xamarin.Essentials.Battery.BatteryInfoChanged += Battery_BatteryInfoChanged;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        private void SetMainPage(ContentPage page)
        {
            var navigationPage = new NavigationPage(page);
            MainPage = navigationPage;
        }
        private ContentPage GetCurrentPage()
        {
            return new LoginPage();
        }
        public void InitializeNavigation()
        {
            var currentPage = GetCurrentPage();
            SetMainPage(currentPage);
        }
        private void RegisterEventHandlers()
        {
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityChanged;
        }
        protected async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            string result = e.IsConnected ? "Nawiązano połączenie z Internetem." : "Utracono połączenie z Internetem";
            await App.Current.MainPage.DisplayAlert("Informacja", result, "OK");
        }

        async void Battery_BatteryInfoChanged(object sender, Xamarin.Essentials.BatteryInfoChangedEventArgs e)
        {
            var level = e.ChargeLevel;
            var state = e.State;
            var source = e.PowerSource;
            switch (state)
            {
                case Xamarin.Essentials.BatteryState.Charging:
                    // Currently charging
                    break;
                case Xamarin.Essentials.BatteryState.Full:
                    // Battery is full
                    break;
                case Xamarin.Essentials.BatteryState.NotCharging:
                    break;
                case Xamarin.Essentials.BatteryState.Discharging:
                    await App.Current.MainPage.DisplayAlert("Informacja", "Niski poziom baterii", "OK");
                    break;
                case Xamarin.Essentials.BatteryState.NotPresent:
                // Battery doesn't exist in device (desktop computer)
                case Xamarin.Essentials.BatteryState.Unknown:
                    // Unable to detect battery state
                    break;
            }
        }

    }
}
