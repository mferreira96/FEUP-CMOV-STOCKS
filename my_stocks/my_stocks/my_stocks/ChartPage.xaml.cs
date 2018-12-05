using my_stocks.model;
using my_stocks.services;
using System;
using System.Collections.Generic;
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

		public ChartPage (List<Company> companies)
		{

			InitializeComponent ();
            FetchData(companies);
		}

        async void FetchData(List<Company> companies)
        {
            Company[] comps;
            try
            {
                comps = await GetCompanies(companies);
            }catch (Exception e)
            {
                comps = new Company[0];
            }

            Debug.WriteLine("Size" + comps.Length);

            chartView.Companies = comps;
            
        }

        async Task<Company[]> GetCompanies(List<Company> companiesList)
        {
            List<string> companies = new List<string>();
            foreach(Company c in companiesList)
            {
                companies.Add(c.symbol);
            }
            return await StockServices.GetStocks(companies.ToArray());
        }
	}
}