using System.Windows;
using System.Windows.Controls;
using FoodPlanner.DataLayer;

namespace FoodPlanner.Tracker
{
    /// <summary>
    /// Interaction logic for Tracker.xaml
    /// </summary>
    public partial class Tracker : UserControl
    {
        private TrackerViewModel ViewModel { get; }
        //private List<FoodPlanEntry> _items = new List<FoodPlanEntry>();

        public Tracker()
        {
            InitializeComponent();

            ViewModel = new TrackerViewModel();
            DataContext = ViewModel;
        }

        private async void decreaseButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.DecreaseEntry((sender as Button).DataContext as FoodPlanEntry);
        }

        private async void increaseButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.IncreaseEntry((sender as Button).DataContext as FoodPlanEntry);
        }

        private void NextPlanButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NextPlan();
        }

        private void PreviousPlanButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PreviousPlan();
        }

        public void Refresh()
        {
            ViewModel.Refresh();
        }
    }
}