using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using FoodPlanner.Services;
using Serilog;
using Serilog.Events;

namespace FoodPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                FoodServiceProvider.Log( LogEventLevel.Error,
                    $"{e.Exception.Message}\n{e.Exception.StackTrace}");
                MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                e.Handled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Everything is horrible", "Fatal Exception");
                MessageBox.Show(exception.Message, "Fatal Exception");

                throw;
            }
        }
    }
}