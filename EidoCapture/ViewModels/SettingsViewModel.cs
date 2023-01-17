using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace EidoCapture.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isCapturing;

        private int _delaySeconds;
        public int DelaySeconds
        {
            get { return _delaySeconds; }
            set
            {
                SetProperty(ref _delaySeconds, value);
            }
        }

        private string _userStorageDirectory;
        public string UserStorageDirectory
        {
            get { return _userStorageDirectory; }
            set
            {
                SetProperty(ref _userStorageDirectory, value);
            }
        }

        [ObservableProperty]
        private List<string> _displays;

        private string _selectedDisplay;
        public string SelectedDisplay
        {
            get { return _selectedDisplay; }
            set
            {
                SetProperty(ref _selectedDisplay, value);
            }
        }

        public SettingsViewModel()
        {
            _isCapturing = false;
            _delaySeconds = 3;
            _userStorageDirectory = MainWindowViewModel.Manager?.GetUserStorageDirectory() ?? string.Empty;
            _displays = MainWindowViewModel.Manager?.Capturer.GetDisplayNames().ToList() ?? new List<string>();
            _selectedDisplay = _displays.FirstOrDefault() ?? string.Empty;
        }
    }
}
