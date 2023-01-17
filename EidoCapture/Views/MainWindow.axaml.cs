using Avalonia.Controls;
using EidoCapture.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace EidoCapture.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = App.Current?.Services?.GetService<MainWindowViewModel>();
        }

        private void OnClosing(object? s, CancelEventArgs a)
        {
            if (DataContext != null && DataContext is MainWindowViewModel viewModel)
            {
                viewModel.Shutdown();
            }
        }
    }
}
