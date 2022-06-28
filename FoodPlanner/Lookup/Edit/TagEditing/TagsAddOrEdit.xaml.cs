using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FoodPlanner.DataLayer;
using FoodPlanner.Lookup.Edit.FoodEditing;

namespace FoodPlanner.Lookup.Edit.TagEditing
{
    /// <summary>
    /// Interaction logic for TagsAddOrEdit.xaml
    /// </summary>
    public partial class TagsAddOrEdit : Window
    {
        public TagsEditViewModel ViewModel { get; set; }
        public bool ClosedProperly { get; set; }

        public TagsAddOrEdit()
        {
            InitializeComponent();
        }

        public TagsAddOrEdit(Tag tag, bool isEdit) : base()
        {
            InitializeComponent();
            ViewModel = new TagsEditViewModel(tag, isEdit);
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
            await ViewModel.SaveTagChanges();
            ClosedProperly = true;
            Close();
        }


        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Sure you want to delete this tag?", "Delete?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            ViewModel.DeleteTag();
            ClosedProperly = true;
            Close();
        }

        private async void TagsAddOrEdit_OnClosing(object sender, CancelEventArgs e)
        {
            if (ClosedProperly) return;
            var result = MessageBox.Show("Would you like to save your changes?", "Save?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) await ViewModel.SaveTagChanges();
        }
    }
}