using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FoodPlanner.DataLayer;
using FoodPlanner.Services;

namespace FoodPlanner.Lookup.Results
{
    /// <summary>
    /// Interaction logic for LookupResults.xaml
    /// </summary>
    public partial class LookupResults : Window
    {
        public List<Food> Results { get; set; }
        public IEnumerable<Food> CurrentResults { get; set; }
        public int Page { get; set; } = 1;
        public int TotalPages => (int) Math.Ceiling((double) Results.Count / PageSize);
        public int PageSize = 6;

        public LookupResults()
        {
            InitializeComponent();
        }


        public LookupResults(IEnumerable<Food> results) : base()
        {
            InitializeComponent();
            Results = results.ToList();
            CurrentResults = Results.Take(PageSize);
            ResultItemsControl.ItemsSource = CurrentResults;
            PageTextBlock.Text = $"Page {Page} / {TotalPages}";
        }

        private void PrevButton_OnClick(object sender, RoutedEventArgs e)
        {
            Page--;
            if (Page < 1) Page = TotalPages;
            PageTextBlock.Text = $"Page {Page} / {TotalPages}";
            CurrentResults = new List<Food>(Results.Skip((Page - 1) * PageSize).Take(PageSize));
            ResultItemsControl.ItemsSource = CurrentResults;
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            Page++;
            if (Page > TotalPages) Page = 1;
            PageTextBlock.Text = $"Page {Page} / {TotalPages}";
            CurrentResults = new List<Food>(Results.Skip((Page - 1) * PageSize).Take(PageSize));
            ResultItemsControl.ItemsSource = CurrentResults;
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var child = VisualTreeHelper.GetChild(border, 0);
            child = VisualTreeHelper.GetChild(child, 0);
            child = VisualTreeHelper.GetChild(child, 0);
            var textBlock = child as TextBlock;
            Process.Start(new ProcessStartInfo
            {
                FileName = $"https://www.google.com/search?q={textBlock.Text}+Recipe",
                UseShellExecute = true
            });
        }
    }
}