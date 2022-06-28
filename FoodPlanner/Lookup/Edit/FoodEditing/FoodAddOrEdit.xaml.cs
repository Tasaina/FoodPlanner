using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using FoodPlanner.DataLayer;

namespace FoodPlanner.Lookup.Edit.FoodEditing
{
    /// <summary>
    /// Interaction logic for FoodAddOrEdit.xaml
    /// </summary>
    public partial class FoodAddOrEdit : Window
    {
        public FoodEditViewModel ViewModel { get; set; }
        public bool ClosedProperly { get; set; }

        public FoodAddOrEdit()
        {
            InitializeComponent();
        }

        public FoodAddOrEdit(Food food, bool isEdit) : base()
        {
            InitializeComponent();
            ViewModel = new FoodEditViewModel(food, isEdit);
            DataContext = ViewModel;
        }

        private async void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Cancel();
            ClosedProperly = true;
            Close();
        }

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            await ViewModel.SaveFoodChanges();
            ClosedProperly = true;
            Close();
        }

        private async void AddTagButton_OnClick(object sender, RoutedEventArgs e)
        {
            await ViewModel.AddTag(TagNameTextBox.Text, TagIsMajorCheckbox.IsChecked);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveTag((sender as Button).DataContext as FoodTag);
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Sure you want to delete this food?", "Delete?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            ClosedProperly = true;
            ViewModel.DeleteFood();
            Close();
        }

        private async void FoodAddOrEdit_OnClosing(object sender, CancelEventArgs e)
        {
            if (ClosedProperly) return;
            var result = MessageBox.Show("Would you like to save your changes?", "Save?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) await ViewModel.SaveFoodChanges();
        }

    }
}