using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FoodPlanner.DataLayer;
using ReactiveUI;

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for ScollableSelector.xaml
    /// </summary>
    public partial class ScrollableSelector : Window
    {
        public ScrollableSelectorViewModel ViewModel { get; private set; }

        public object? Selection { get; set; }

        public ScrollableSelector(IEnumerable<object> items) : base()
        {
            InitializeComponent();
            ViewModel = new ScrollableSelectorViewModel(items);
            DataContext = ViewModel;
        }

        public ScrollableSelector()
        {
        }


        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock;
            if (sender is TextBlock senderBlock) textBlock = senderBlock;
            else
            {
                var border = sender as Border;
                var textBlockChild = VisualTreeHelper.GetChild(border, 0) as TextBlock;
                textBlock = textBlockChild;
            }

            Selection = ViewModel.GetSelection(textBlock);
            if (Selection == null) return;
            Close();
        }
    }
}