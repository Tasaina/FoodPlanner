using System;
using System.Windows;
using System.Windows.Controls;

namespace FoodPlanner.Recommendation
{
    /// <summary>
    /// Interaction logic for Recommendation.xaml
    /// </summary>
    public partial class Recommendation : UserControl
    {
        private RecommendationViewModel ViewModel { get; set; }

        public Recommendation()
        {
            Loaded += InitializeValues;
            InitializeComponent();
            ViewModel = new RecommendationViewModel();
            DataContext = ViewModel;
        }


        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
              await ViewModel.UpdateFood();
        }

        private async void InitializeValues(object? sender, EventArgs e)
        {
            await ViewModel.Initialize();
        }
    }
}