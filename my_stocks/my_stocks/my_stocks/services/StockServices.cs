using my_stocks.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace my_stocks.services
{
    class StockServices
    {
        struct CompanyList
        {
            public Company[] companies;
        }
        public async static Task<Company[]> GetStocks(string[] names, bool weekly= true)
        {
            string companies = String.Join(",", names);
            CompanyList list = await WebInterface.getInstance().Get<CompanyList>(String.Format("/stocks/{0}/{1}",weekly? "week" : "month", companies));
            return list.companies;
        }
    }
}
