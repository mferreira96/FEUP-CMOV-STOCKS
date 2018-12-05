using my_stocks.model;
using my_stocks.services;
using my_stocks.view;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace my_stocks
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChartPage : ContentPage
	{
        private bool loading = true;
        public bool Loading
        {
            get
            {
                return loading;
            }
            set
            {
                if(loading != value)
                    loading = value;
                OnPropertyChanged();
            }
        }

		public ChartPage (List<Company> companies, bool byWeek)
		{

			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            loading = true;
            FetchData(companies, byWeek);
            Title = "Stock Evolution";
            
		}

        async void FetchData(List<Company> companies, bool byWeek)
        {
            Company[] comps;
            try
            {
                comps = await GetCompanies(companies, byWeek);
            }catch (Exception e)
            {
                comps = new Company[0];
            }

            Debug.WriteLine("Size" + comps.Length);

            chartView.Companies = comps;

            ObservableCollection<Company> cmps = new ObservableCollection<Company>();
            int i = 0;
            foreach(Company c in comps)
            {
                c.name = companies[i].name;
                c.Index = i++;
                cmps.Add(c);
            }
            Loading = false;

            companiesList.SelectionMode = ListViewSelectionMode.None;
            companiesList.ItemsSource = cmps;
            companiesList.ItemTemplate = new DataTemplate(typeof(CompanyCellChartTemplate));
            companiesList.SelectionMode = ListViewSelectionMode.Single;
        }

        async Task<Company[]> GetCompanies(List<Company> companiesList, bool byWeek)
        {
            List<string> companies = new List<string>();
            foreach(Company c in companiesList)
            {
                companies.Add(c.symbol);
            }
            return await StockServices.GetStocks(companies.ToArray(), byWeek);
        }
	}
}