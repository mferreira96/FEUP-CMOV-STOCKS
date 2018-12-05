using my_stocks.data;
using my_stocks.model;
using my_stocks.viewModel;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace my_stocks.view
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListStocks : ContentPage
	{
        private List<Company> selectedCompanies;
        private ListCompanies listCompanies;

        public ListStocks()
		{

            InitializeComponent();
            Title = "Companies";
            listCompanies = new ListCompanies();

            companiesList.ItemsSource = listCompanies.Companies;
            companiesList.ItemTemplate = new DataTemplate(typeof(CompanyCellTemplate));
            companiesList.RefreshCommand = new Command(() =>
            {
                listCompanies.BuildList();
                companiesList.IsRefreshing = false;
            });
            companiesList.SelectionMode = ListViewSelectionMode.Single;
            companiesList.ItemTapped += OnTapEvent;

            compareButton.Clicked += OnButtonClick;
            cancelButton.Clicked += OnButtonClick;
            viewStats.Clicked += OnButtonClick;

            selectedCompanies = new List<Company>();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.ClassId.Equals(compareButton.ClassId))
            {
                await Navigation.PushAsync(new ChartPage(selectedCompanies));
            }
            else if (btn.ClassId.Equals(cancelButton.ClassId))
            {
                foreach(Company c in selectedCompanies)
                {
                    c.Selected = false;
                }
                selectedCompanies.Clear();
                ButtonsVisibility(0);
            }
            else if (btn.ClassId.Equals(viewStats.ClassId))
            {
                //navigate
            }
        }

        private void OnTapEvent(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            Company selectedItem = e.Item as Company;
            
            if (selectedItem.Selected)
            {
                selectedItem.Selected = false;
                selectedCompanies.Remove(selectedItem);
            }
            else
            {
                selectedItem.Selected = true;
                selectedCompanies.Add(selectedItem);
            }


            int totalSelected = selectedCompanies.Count;


            ButtonsVisibility(totalSelected);
        }

        private void ButtonsVisibility(int totalSelected)
        {
            if (totalSelected >= 2)
            {
                Console.WriteLine("more than one selected");
                multiple.IsVisible = true;
                single.IsVisible = false;
            }
            else if (totalSelected == 1)
            {
                Console.WriteLine("one selected");
                multiple.IsVisible = false;
                single.IsVisible = true;
            }
            else
            {
                multiple.IsVisible = false;
                single.IsVisible = false;
                Console.WriteLine("none");
            }
        }
    }
}