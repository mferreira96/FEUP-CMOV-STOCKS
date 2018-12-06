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

        private bool loading = true;
        public bool Loading
        {
            get
            {
                return loading;
            }
            set
            {
                if (loading != value)
                    loading = value;
                OnPropertyChanged();
            }
        }
        
        private List<Company> selectedCompanies;
        private ListCompanies listCompanies;
        private bool byWeek = true;

        public ListStocks()
		{

            Loading = true;
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
            Loading = false;
            companiesList.SelectionMode = ListViewSelectionMode.Single;
            companiesList.ItemTapped += OnTapEvent;

            compareButton.Clicked += OnButtonClick;
            cancelButton.Clicked += OnButtonClick;

            sortByButton.Clicked += (a, b) =>
            {
                byWeek = !byWeek;
                sortByButton.Text = byWeek ? "By Week" : "By Month";
            };

            selectedCompanies = new List<Company>();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.ClassId.Equals(compareButton.ClassId))
            {
                await Navigation.PushAsync(new ChartPage(selectedCompanies, byWeek));
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
            else if(selectedCompanies.Count < 4)
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
                compareButton.Text = "Compare";
                cancelButton.IsVisible = true;
            }
            else if (totalSelected == 1)
            {
                Console.WriteLine("one selected");
                multiple.IsVisible = true;
                compareButton.Text = "Analyse";
                cancelButton.IsVisible = false;
            }
            else
            {
                multiple.IsVisible = false;
                cancelButton.IsVisible = false;
                Console.WriteLine("none");
            }
        }
    }
}