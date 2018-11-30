using my_stocks.view;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace my_stocks
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new ListStocks();
            MainPage = new Chart();
        }

        public static Page GetMainPage()
        {
            var companies = new ListStocks();
            return new NavigationPage(companies);
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
    }
}
