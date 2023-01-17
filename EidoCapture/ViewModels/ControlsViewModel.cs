using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace EidoCapture.ViewModels
{
    public partial class ControlsViewModel : ObservableObject
    {
        private const string ACTIVE_COLOR = "Red";
        private const string INACTIVE_COLOR = "White";

        public RelayCommand OnActivateCaptureCommand { get; }
        public RelayCommand OnDeactivateCaptureCommand { get; }

        [ObservableProperty]
        private string _activateBtnColor;

        [ObservableProperty]
        private string _deactivateBtnColor;

        public ControlsViewModel()
        {
            _activateBtnColor = INACTIVE_COLOR;
            _deactivateBtnColor = ACTIVE_COLOR;

            OnActivateCaptureCommand = new RelayCommand(() => OnActivateCapture());
            OnDeactivateCaptureCommand = new RelayCommand(() => OnDeactivateCapture());
        }

        private void OnActivateCapture()
        {
            SettingsViewModel? settingsVM = App.Current?.Services?.GetService<SettingsViewModel>();

            if (settingsVM != null)
            {
                ActivateBtnColor = ACTIVE_COLOR;
                DeactivateBtnColor = INACTIVE_COLOR;

                settingsVM.IsCapturing = true;
                MainWindowViewModel.Manager?.Capturer.Activate(settingsVM.DelaySeconds, settingsVM.SelectedDisplay);
            }
        }

        private void OnDeactivateCapture()
        {
            ActivateBtnColor = INACTIVE_COLOR;
            DeactivateBtnColor = ACTIVE_COLOR;

            SettingsViewModel? settingsVM = App.Current?.Services?.GetService<SettingsViewModel>();

            if (settingsVM != null)
            {
                settingsVM.IsCapturing = false;
                MainWindowViewModel.Manager?.Capturer.Deactivate();
            }
        }
    }
}
