using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace EidoCapture.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isCapturing;

        [ObservableProperty]
        private int _delaySeconds;

        [ObservableProperty]
        private List<string> _displays;

        [ObservableProperty]
        private string _selectedDisplay;

        private string _userStorageDirectory;
        public string UserStorageDirectory
        {
            get { return _userStorageDirectory; }
            set
            {
                SetProperty(ref _userStorageDirectory, value);
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
