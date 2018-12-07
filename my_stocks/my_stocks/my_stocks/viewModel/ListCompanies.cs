using my_stocks.model;
using my_stocks.services;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace my_stocks.viewModel
{
    public class SelectableObservableCollection<T> : ObservableCollection<T>
    {
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

    }
    public class ListCompanies
    {
        private ObservableCollection<Company> companies;

        public Action OnFinished;

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
            WebInterface webInterface = WebInterface.GetInstance();
            CompaniesList companiesRetrieved = null;
            try
            {
                 companiesRetrieved = await webInterface.Get<CompaniesList>("/companies");

            } catch(Exception e)
            {
                
                companiesRetrieved = new CompaniesList
                {
                    companies = new Company[]{
                    new Company("TEST", 20, "123", 0.3, 22.22),
                    new Company("TEST", 20, "123", 0.3, 22.22)
                    }
                };

                Console.WriteLine(e.Message);
            }
      
            if (companiesRetrieved != null)
            {
                this.companies.Clear();

                foreach (var company in companiesRetrieved.companies)
                {
                    this.companies.Add(company);
                }
            }
            OnFinished?.Invoke();
        }
    }
}
