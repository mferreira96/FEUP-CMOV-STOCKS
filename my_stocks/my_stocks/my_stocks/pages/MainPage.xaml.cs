using my_stocks.services;
using my_stocks.view;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Threading.Tasks;
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

            config.Clicked += (a, b) =>
            {
                url.IsVisible = !url.IsVisible;
            };

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
            
            start.Clicked += async (a, b) =>
            {
                bool networkConnection = await CheckInternetConnectionAsync();
                if (VerifyUrl() && networkConnection)
                {
                    SaveURL();
                    await Navigation.PushAsync(new ListStocks());
                }
            };
        }

        private async Task<bool> CheckInternetConnectionAsync()
        {
            if (CrossConnectivity.Current.IsConnected == true)
            {

                int size = url.Text.Length;
                string [] splitedValues = url.Text.Split(':');
                string urlBase = splitedValues[0] + ':' + splitedValues[1];
                int port = int.Parse(splitedValues[2]);
                Console.WriteLine("port" + port);
                Console.WriteLine("url" + urlBase);
                bool pingResult = await CrossConnectivity.Current.IsRemoteReachable(urlBase, port);

                if(pingResult == false)
                {
                    errorConnection.Text = "Server down!";
                    return false;
                }
                else
                {
                    errorConnection.Text = "";
                    return true;
                } 
            }
            else
            {
                errorConnection.Text = "Check your internet connection";
                return false;
            }
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
