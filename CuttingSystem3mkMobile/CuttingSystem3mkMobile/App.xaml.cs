using CuttingSystem3mkMobile.Pages;
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
    }
}
