using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FoodPlanner.DataLayer;
using FoodPlanner.Lookup.Edit;
using FoodPlanner.Lookup.Edit.FoodEditing;
using FoodPlanner.Lookup.Edit.TagEditing;
using FoodPlanner.Services;

namespace FoodPlanner.Lookup.Settings
{
    /// <summary>
    /// Interaction logic for LookupOptions.xaml
    /// </summary>
    public partial class LookupSettings : Window
    {
        private readonly IFoodService _foodService;
        private readonly ITagService _tagService;

        public LookupSettings()
        {
            _foodService = FoodServiceProvider.GetService<IFoodService>();
            _tagService = FoodServiceProvider.GetService<ITagService>();
            InitializeComponent();
            Height = 250;
            Width = 350;
        }

        private async void FoodSearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchRequest = FoodSearchRequest.Empty;
            searchRequest.IncludeTerms = FoodSearchTextbox.Text.Split(", ");
            var results = (await _foodService.Lookup(searchRequest, 100)).ToList();
            switch (results.Count)
            {
                case 0:
                    MessageBox.Show("Nothing found");
                    return;
                case 1:
                    ShowFoodEditorDialog(results.First());
                    break;
                default:
                {
                    var scrollable = new ScrollableSelector(results);
                    scrollable.ShowDialog();
                    if (scrollable.Selection == null) return;
                    ShowFoodEditorDialog(scrollable.Selection as Food);
                    break;
                }
            }
        }

        private static void ShowFoodEditorDialog(Food foodToEdit)
        {
            var editItemWindow = new FoodAddOrEdit(foodToEdit, true);
            editItemWindow.ShowDialog();
        }

        private void TagSearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            var results = _tagService.GetMatching(TagSearchTextbox.Text).ToList();
            switch (results.Count)
            {
                case 0:
                    MessageBox.Show("Nothing found");
                    return;
                case 1:
                    ShowTagEditorDialog(results.First());
                    break;
                default:
                {
                    var scrollable = new ScrollableSelector(results);
                    scrollable.ShowDialog();
                    if (scrollable.Selection == null) return;
                    ShowTagEditorDialog(scrollable.Selection as Tag);
                    break;
                }
            }
        }

        private void ShowTagEditorDialog(Tag tagToEdit)
        {
            var editItemWindow = new TagsAddOrEdit(tagToEdit, true);
            editItemWindow.ShowDialog();
        }

        private void NewTagButton_OnClick(object sender, RoutedEventArgs e)
        {
            var editItemWindow = new TagsAddOrEdit(new Tag(), false);
            editItemWindow.ShowDialog();
        }

        private void NewFoodButton_OnClick(object sender, RoutedEventArgs e)
        {
            var editItemWindow = new FoodAddOrEdit(new Food(), false);
            editItemWindow.ShowDialog();
        }
    }
}