using EidoCapture.Data;

namespace EidoCapture.Business
{
    public static class Manager
    {
        public static string? AppVersion { get; set; }

        public static string GetCurrentUserStorageDirectory()
        {
            return CRUD.GetCurrentUserStorageDirectory();
        }

        public static async Task UpdateUserStorageDirectoryAsync(string newUserStorageDirectory)
        {
            await CRUD.UpdateUserStorageDirectoryAsync(newUserStorageDirectory);
        }
    }
}
