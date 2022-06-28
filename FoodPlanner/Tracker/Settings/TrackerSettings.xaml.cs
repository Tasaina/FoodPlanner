using System;
using System.Windows;
using System.Windows.Controls;
using FoodPlanner.DataLayer;

namespace FoodPlanner.Tracker.Settings
{
    /// <summary>
    /// Interaction logic for TrackerOptions.xaml
    /// </summary>
    public partial class TrackerSettings : Window
    {
        public TrackerSettingsViewModel ViewModel { get; set; }

        public TrackerSettings()
        {
            ContentRendered += InitializeValues;
            InitializeComponent();
            ViewModel = new TrackerSettingsViewModel();
            DataContext = ViewModel;
        }

        private async void InitializeValues(object? sender, EventArgs e)
        {
            ViewModel.InitializeValues();
        }


        private async void decreaseButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.DecreaseEntry((sender as Button).DataContext as FoodPlanEntry);
        }

        private async void increaseButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.IncreaseEntry((sender as Button).DataContext as FoodPlanEntry);
        }

        private void NewEntryButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.NewEntry();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Delete((sender as Button).DataContext as FoodPlanEntry);
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Edit((sender as Button).DataContext as FoodPlanEntry);
        }
    }
}