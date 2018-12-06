using my_stocks.services;
using my_stocks.view;
using System;
using Xamarin.Forms;

namespace my_stocks
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
            string source =  "my_stocks.resources.logo.png";
            icon.Source =  ImageSource.FromResource(source);
            NavigationPage.SetHasNavigationBar(this, false);

            try
            {
                String previousUrl = Application.Current.Properties["url"].ToString();
                if (previousUrl != null)
                {
                    url.Text = previousUrl;
                }
                else
                {
                    url.Text = "http://127.125.125.125:8080";
                }
            } catch(Exception e)
            {
                url.Text = "http://127.125.125.125:8080";
            }
            
            start.Clicked += (a, b) =>
            {
                if (VerifyUrl())
                {
                    SaveURL();
                    Navigation.PushAsync(new ListStocks());
                }
            };
        }

        private void SaveURL()
        {
            Application.Current.Properties["url"] = url.Text;
            Application.Current.SavePropertiesAsync();
        }

        private bool VerifyUrl()
        {
            if (url.Text.Length == 0)
                return false;

            return Uri.IsWellFormedUriString(url.Text, UriKind.RelativeOrAbsolute);
        }
    }
}
