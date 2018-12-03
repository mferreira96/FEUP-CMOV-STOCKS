using my_stocks.data;
using my_stocks.model;
using my_stocks.services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace my_stocks.viewModel
{
    public class ListCompanies
    {
        private ObservableCollection<Company> companies;

        public ObservableCollection<Company> Companies
        {
            get { return companies; }
            set
            {
                companies = value;
            }
        }

        public ListCompanies()
        {
            this.companies = new ObservableCollection<Company>();
            this.BuildList();    
        }

        public async void BuildList()
        {
            WebInterface webInterface = WebInterface.getInstance();
            CompaniesList companiesRetrieved = await webInterface.Get<CompaniesList>("/companies");

            if (companiesRetrieved != null)
            {
                this.companies.Clear();

                foreach (var company in companiesRetrieved.companies)
                {
                    Console.WriteLine("company = " + company);
                    this.companies.Add(company);
                }
            }
        }
    }
}
