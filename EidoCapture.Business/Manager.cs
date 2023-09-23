using EidoCapture.Data;

namespace EidoCapture.Business
{
    public static class Manager
    {
        public const string PRIVACY_URL = "https://github.com/aaron-salisbury/EidoCapture/blob/master/PrivacyPolicy.md";
        public const string ISSUES_URL = "https://github.com/aaron-salisbury/EidoCapture/issues";

        public static string GetCurrentUserStorageDirectory()
        {
            return CRUD.GetCurrentUserStorageDirectory();
        }

        public static void UpdateUserStorageDirectory(string newUserStorageDirectory)
        {
            Task.Run(() => CRUD.UpdateUserStorageDirectoryAsync(newUserStorageDirectory));
        }
    }
}
