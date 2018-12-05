using my_stocks.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace my_stocks.view
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompanyCellChart : ContentView
	{
        public CompanyCellChart()
        {
            InitializeComponent();
        }

    }

    public class CompanyCellChartTemplate : ViewCell
    {
        public CompanyCellChartTemplate ()
        {
            View = new CompanyCellChart();
        }
    }
}