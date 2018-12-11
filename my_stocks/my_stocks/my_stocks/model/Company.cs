using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace my_stocks.model
{
    public class Company : INotifyPropertyChanged
    {
        public String name { get; set;}
        
        public double LastPrice {
            get { return lastPrice; }
            set
            {
                if(lastPrice != value)
                {
                    lastPrice = value;
                    OnPropertyChanged();
                }
            }
        }
     
        public Double lastPrice { get; set;}

        public String symbol { get; set;}

        public Double netChange { get; set; }

        private int index;

        public int Index { get; set; }

        public Double percentChange { get; set; }

        private bool selected = false;
        public Boolean Selected {
            get { return selected; }
            set
            {
                if (selected != value)
                    selected = value;
                OnPropertyChanged();
            }
        }
        public StockData[] History;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Company(Company company)
        {
            this.name = company.name;
            this.lastPrice = company.lastPrice;
            this.symbol = company.symbol;
            this.selected = company.selected;
            this.netChange = company.netChange;
            this.percentChange = company.percentChange;
            this.History = company.History;
        }

        public Company(String name, Double lastPrice, String symbol, Double netChange, Double percentChange)
        {
            this.name = name;
            this.lastPrice = lastPrice;
            this.symbol = symbol;
            this.selected = false;
            this.netChange = netChange;
            this.percentChange = percentChange;
        }

        public Company()
        {
            this.selected = false;
        }
    }
}
