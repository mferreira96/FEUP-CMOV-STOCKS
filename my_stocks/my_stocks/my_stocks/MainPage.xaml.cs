using my_stocks.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace my_stocks
{
    public partial class MainPage : ContentPage
    {
        public int Dummy = 0;

        public MainPage()
        {
            InitializeComponent();
            
            string source =  "my_stocks.resources.logo.png";
            icon.Source =  ImageSource.FromResource(source);
            NavigationPage.SetHasNavigationBar(this, false);
            start.Clicked += (a, b) =>
            {
                Navigation.PushAsync(new ListStocks());
            };
        }
    }
}
