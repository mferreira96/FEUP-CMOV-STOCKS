using System;
using System.Collections.Generic;
using System.Text;

namespace my_stocks.model
{
    public class Company
    {
        public String name { get; set;}
     
        public Double lastPrice { get; set;}

        public String symbol { get; set;}

        public Boolean Selected { get; set; }

        public StockData[] History; 

        public Company(String name, Double lastPrice, String symbol)
        {
            this.name = name;
            this.lastPrice = lastPrice;
            this.symbol = symbol;
            this.Selected = false;
        }

        public Company()
        {
            this.Selected = false;
        }
    }
}
