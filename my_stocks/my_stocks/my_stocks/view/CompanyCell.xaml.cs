using System;
using System.Collections.Generic;
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
		public CompanyCell ()
		{
			InitializeComponent ();
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