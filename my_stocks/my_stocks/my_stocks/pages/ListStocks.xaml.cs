﻿using my_stocks.model;
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
        private int sortBy = 0;
        private string[] sortName = { "Day", "Week", "Month" };

        public ListStocks()
		{

            Loading = true;
            InitializeComponent();
            Title = "Companies";
            listCompanies = new ListCompanies();

            listCompanies.OnFinished = () =>
            {
                Loading = false;
            };

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

            sortByButton.Text = "By " + sortName[sortBy];
            sortByButton.Clicked += (a, b) =>
            {
                sortBy = (sortBy + 1) % sortName.Length;
                sortByButton.Text = "By " + sortName[sortBy];
            };

            selectedCompanies = new List<Company>();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.ClassId.Equals(compareButton.ClassId))
            {
                await Navigation.PushAsync(new ChartPage(selectedCompanies, sortName[sortBy].ToLower()));
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
                multiple.IsVisible = true;
                compareButton.Text = "Compare";
                cancelButton.IsVisible = true;
            }
            else if (totalSelected == 1)
            {
                multiple.IsVisible = true;
                compareButton.Text = "Analyse";
                cancelButton.IsVisible = false;
            }
            else
            {
                multiple.IsVisible = false;
                cancelButton.IsVisible = false;
            }
        }
    }
}