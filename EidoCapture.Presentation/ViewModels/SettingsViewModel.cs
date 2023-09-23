using EidoCapture.Business;
using EidoCapture.Presentation.Base;

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
                Manager.UpdateUserStorageDirectory(_storageFolderPath);
            }
        }

        public string PrivacyPolicyURL => Manager.PRIVACY_URL;

        public string IssuesURL => Manager.ISSUES_URL;
    }
}
