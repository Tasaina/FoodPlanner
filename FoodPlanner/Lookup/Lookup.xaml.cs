using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FoodPlanner.DataLayer;
using FoodPlanner.Lookup.Results;
using FoodPlanner.Services;

namespace FoodPlanner.Lookup
{
    /// <summary>
    /// Interaction logic for Lookup.xaml
    /// </summary>
    public partial class Lookup : UserControl
    {
        private readonly IFoodService _foodService;
        public LookupViewModel ViewModel { get; set; }


        public Lookup()
        {
            InitializeComponent();
            ViewModel = new LookupViewModel();
            DataContext = ViewModel;
            _foodService = FoodServiceProvider.GetService<IFoodService>();
        }


        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            var foodSearchRequest = new FoodSearchRequest()
            {
                MealType = (MealType) MealTypeBox.SelectedIndex,
                MealSize = (MealSize) MealSizeBox.SelectedIndex,
                FlavorProfile = (FlavorProfile) FlavorProfileBox.SelectedIndex,
                Temperature = (Temperature) TemperatureBox.SelectedIndex,
                IncludeTerms = IncludeTermsBox.Text.Split(", "),
                ExcludeTerms = ExcludeTermsBox.Text.Split(", "),
                Page = 1
            };
            foodSearchRequest.RemoveEmptySearchTerms();
            var results = (await _foodService.Lookup(foodSearchRequest)).ToList();

            if (!results.Any()) MessageBox.Show("Nothing Found");
            else new LookupResults(results).Show();
        }

        private void ResetButton_OnClick(object sender, RoutedEventArgs e)
        {
            MealTypeBox.SelectedItem = null;
            TemperatureBox.SelectedItem = null;
            MealSizeBox.SelectedItem = null;
            FlavorProfileBox.SelectedItem = null;
            ExcludeTermsBox.Text = "";
            IncludeTermsBox.Text = "";
        }
    }
}