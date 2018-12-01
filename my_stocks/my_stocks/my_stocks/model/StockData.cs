using System;
using System.Collections.Generic;
using System.Text;

namespace my_stocks.model
{
    public class StockData
    {
        public float Value;
        public DateTime Date;
        
        public StockData(float value, string date)
        {
            Value = value;
            Date = DateTime.Parse(date);
        }
    }
}
