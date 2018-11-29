using my_stocks.data;
using my_stocks.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
            companies = new ObservableCollection<Company>();
            Test _context = new Test();

            foreach ( var company in _context.Companies)
            {
                Companies.Add(company);
            }
        }
    }
}
