using CommunityToolkit.Mvvm.ComponentModel;
using EidoCapture.Business;
using EidoCapture.Presentation.Base;
using System.Threading.Tasks;

namespace EidoCapture.Presentation.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        private string _storageFolderPath = Manager.GetCurrentUserStorageDirectory();
        public string StorageFolderPath
        {
            get { return _storageFolderPath; }
            set
            {
                SetProperty(ref _storageFolderPath, value);
                Task.Run(() => Manager.UpdateUserStorageDirectoryAsync(_storageFolderPath));
            }
        }

        [ObservableProperty]
        private string _appDisplayName;

        [ObservableProperty]
        private string _copyHolder;

        [ObservableProperty]
        private string _privacyURL;

        [ObservableProperty]
        private string _issuesURL;

        [ObservableProperty]
        private string _appDescription;

        public string ApplicationInfo => $"{AppDisplayName} - {Manager.AppVersion}";

        public SettingsViewModel()
        {
            _appDisplayName = AppInfo.AppDisplayName;
            _copyHolder = AppInfo.CopyHolder;
            _privacyURL = AppInfo.PrivacyURL;
            _issuesURL = AppInfo.IssuesURL;
            _appDescription = AppInfo.AppDescription;
        }
    }
}
