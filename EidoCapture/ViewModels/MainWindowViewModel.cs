using CommunityToolkit.Mvvm.ComponentModel;
using EidoCapture.Business;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace EidoCapture.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public static Manager? Manager { get; private set; }

        [ObservableProperty]
        private object? _settingsView;

        [ObservableProperty]
        private object? _controlsView;

        public MainWindowViewModel()
        {
            Manager = new Manager(Log.Logger);

            _settingsView = App.Current?.Services?.GetService<SettingsViewModel>();
            _controlsView = App.Current?.Services?.GetService<ControlsViewModel>();
        }

        public void Shutdown()
        {
            Manager?.Shutdown();
        }
    }
}
