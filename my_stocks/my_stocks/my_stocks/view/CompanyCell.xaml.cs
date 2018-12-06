
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


}