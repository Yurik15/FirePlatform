using System;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Common.Entities;
using FirePlatform.Mobile.Common.Implements;
using FirePlatform.Mobile.Common.PushNotification;
using FirePlatform.Mobile.PageModels;
using FirePlatform.Mobile.Pages;
using FreshMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FirePlatform.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var startPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var basicNavContainer = new FreshNavigationContainer(startPage);
            Application.Current.MainPage = basicNavContainer;

            FreshIOC.Container.Register<IParser<ArrayOfItemGroupSer>, ParserXml<ArrayOfItemGroupSer>>();
            CrossPushNotification.Current.OnTokenRefresh += OnTokenRefresh;
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

        void OnTokenRefresh(object source, Common.PushNotification.Abstractions.PushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"TOKEN : {e.Token}");
        }
    }
}
