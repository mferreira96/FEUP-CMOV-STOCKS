using System;


namespace my_stocks.model
{
    public class Company
    {
        public String name { get; set;}
     
        public Double lastPrice { get; set;}

        public String symbol { get; set;}

        public Double netChange { get; set; }

        public Double percentChange { get; set; }

        public Boolean Selected { get; set; }

        public StockData[] History; 

        public Company(String name, Double lastPrice, String symbol, Double netChange, Double percentChange)
        {
            this.name = name;
            this.lastPrice = lastPrice;
            this.symbol = symbol;
            this.Selected = false;
            this.netChange = netChange;
            this.percentChange = percentChange;
        }

        public Company()
        {
            this.Selected = false;
        }
    }
}
