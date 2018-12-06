
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