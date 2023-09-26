using EidoCapture.Data.Attributes;
using EidoCapture.Data.Domains;
using System.Diagnostics;
using System.Text.Json;

namespace EidoCapture.Data
{
    public static class CRUD
    {
        private static InternalStorage _internalStorage = Task.Run(() => InitializeInternalStorageAsync()).Result;

        public static void CreateImage(byte[] imageBuffer, string fileName)
        {
            try
            {
                string path = Path.Combine(_internalStorage.UserStorageDirectory, "Images", fileName);

                FileInfo file = new(path);
                file.Directory?.Create();

                File.WriteAllBytes(path, imageBuffer);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to save image: {e.Message}", "ERROR");
            }
        }

        public static async Task<T?> ReadDataAsync<T>()
        {
            try
            {
                string filePath = Path.Combine(GetIODirectoryPath<T>(), GetJsonFileNameForType(typeof(T)));

                if (File.Exists(filePath))
                {
                    using (FileStream openStream = File.OpenRead(filePath))
                    {
                        return await JsonSerializer.DeserializeAsync<T>(openStream);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to read from file system: {e.Message}", "ERROR");
            }

            return default;
        }

        public static async Task UpdateDataAsync<T>(T data)
        {
            if (data != null)
            {
                try
                {
                    FileInfo file = new FileInfo(Path.Combine(GetIODirectoryPath<T>(), GetJsonFileNameForType(typeof(T))));

                    if (file != null && file.Directory != null)
                    {
                        file.Directory.Create();

                        using (FileStream createStream = File.Create(file.FullName))
                        {
                            await JsonSerializer.SerializeAsync<T>(createStream, data);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to write to file system: {e.Message}", "ERROR");
                }
            }
        }

        public static async Task<bool> UpdateUserStorageDirectoryAsync(string newUserDirectory)
        {
            bool isSuccessful = false;

            try
            {
                if (!string.IsNullOrEmpty(newUserDirectory))
                {
                    _internalStorage.UserStorageDirectory = newUserDirectory;
                    await UpdateDataAsync<InternalStorage>(_internalStorage);

                    isSuccessful = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to update internal storage: {e.Message}", "ERROR");
            }

            return isSuccessful;
        }

        public static void DeleteDomain<T>()
        {
            try
            {
                string filePath = Path.Combine(GetIODirectoryPath<T>(), GetJsonFileNameForType(typeof(T)));

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to delete file: {e.Message}", "ERROR");
            }
        }

        public static string GetCurrentUserStorageDirectory()
        {
            return _internalStorage.UserStorageDirectory;
        }

        private static async Task<InternalStorage> InitializeInternalStorageAsync()
        {
            InternalStorage? internalStorage = ReadDataAsync<InternalStorage?>()?.Result;

            if (internalStorage != null)
            {
                return internalStorage;
            }
            else
            {
                internalStorage = new InternalStorage() { UserStorageDirectory = GetAppDirectoryPath() };

                await UpdateDataAsync<InternalStorage>(internalStorage);

                return internalStorage;
            }
        }

        private static string GetIODirectoryPath<T>()
        {
            if (typeof(T) == typeof(InternalStorage))
            {
                return GetAppDirectoryPath();
            }
            else
            {
                return GetCurrentUserStorageDirectory();
            }
        }

        private static string GetAppDirectoryPath()
        {
            string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string? assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string? appName = assemblyName?.Substring(0, assemblyName.IndexOf('.'));

            return !string.IsNullOrEmpty(appName) ? Path.Combine(appPath, appName) : appPath;
        }

        private static string GetJsonFileNameForType(Type domainType)
        {
            Attribute[] attributes = Attribute.GetCustomAttributes(domainType);
            Attribute? baseNameAttribute = attributes.Where(a => a is BaseNameAttribute).FirstOrDefault();
            if (baseNameAttribute != null)
            {
                return $"{((BaseNameAttribute)baseNameAttribute).BaseName}.json";
            }

            string typeName = domainType.ToString();
            int pos = typeName.LastIndexOf('.') + 1;
            string objectName = typeName.Substring(pos, typeName.Length - pos);

            return $"{objectName}.json";
        }
    }
}
