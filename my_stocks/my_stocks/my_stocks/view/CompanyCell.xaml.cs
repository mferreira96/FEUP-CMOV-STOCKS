﻿using my_stocks.model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace my_stocks.view
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompanyCell : ContentView
	{
        public CompanyCell()
        {
            InitializeComponent();
        }
	}

    public class CompanyCellTemplate : ViewCell
    {
        public CompanyCellTemplate ()
        {
            View = new CompanyCell();
            
        }
    }

    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double netChangeValue = (Double)value;

            if (netChangeValue >= 0)
            {
                return Color.Green;
            }
            else
            {
                return Color.Red;
            }

    }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}