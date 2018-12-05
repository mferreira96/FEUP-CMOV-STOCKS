using System;
using System.Collections.Generic;
using System.Text;

namespace my_stocks.model
{
    class CompaniesList
    {
        public Company[] companies { get; set; }

        public CompaniesList() { }

        public CompaniesList(Company[] companies)
        {
            this.companies = companies;
        }
    }
}
