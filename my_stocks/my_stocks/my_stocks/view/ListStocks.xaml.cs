using my_stocks.data;
using my_stocks.model;
using my_stocks.viewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace my_stocks.view
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListStocks : ContentPage
	{       

        public ListStocks()
		{

            InitializeComponent();
            Title = "Companies";
            ListCompanies listCompanies = new ListCompanies();
            
            companiesList.ItemsSource = listCompanies.Companies;
            companiesList.RefreshCommand = new Command(() =>
            {
                listCompanies.BuildList();
                companiesList.IsRefreshing = false;
            });
		}
	}
}