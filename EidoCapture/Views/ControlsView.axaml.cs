using Avalonia.Controls;
using EidoCapture.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace EidoCapture.Views
{
    public partial class ControlsView : UserControl
    {
        public ControlsView()
        {
            InitializeComponent();

            DataContext = App.Current?.Services?.GetService<ControlsViewModel>();
        }
    }
}
