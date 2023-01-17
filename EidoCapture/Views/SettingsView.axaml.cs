using Avalonia.Controls;
using EidoCapture.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace EidoCapture.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();

            DataContext = App.Current?.Services?.GetService<SettingsViewModel>();
        }
    }
}
