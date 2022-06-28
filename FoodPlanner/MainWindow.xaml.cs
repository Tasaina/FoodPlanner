using System;
using System.Collections.Generic;
using FoodPlanner.Services;
using System.Windows;
using FoodPlanner.DataLayer;
using FoodPlanner.Lookup.Settings;
using FoodPlanner.Tracker.Settings;

namespace FoodPlanner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            FoodServiceProvider.Initialize();
            InitializeComponent();
        }

        private void TrackerSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var trackerSettingsPage = new TrackerSettings();
            trackerSettingsPage.ShowDialog();
            Tracker.Refresh();
        }

        private void LookupSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var lookupSettingsPage = new LookupSettings();
            lookupSettingsPage.ShowDialog();
        }
    }
}