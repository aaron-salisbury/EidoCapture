using Serilog;
using System;
using System.Globalization;
using System.IO;

namespace EidoCapture.Data
{
    public static class CRUD
    {
        public static string AppDirectoryPath { get; set; } = GetAppDirectoryPath();

        public static void CreateImage(byte[] imageBuffer, string fileName, ILogger logger)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                fileName = fileName ?? currentDate.ToString("yyyy-MM-dd HH-mm-ss", DateTimeFormatInfo.InvariantInfo) + ".jpg";
                string thisDaysImageFolder = currentDate.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
                string path = Path.Combine(AppDirectoryPath, "Images", thisDaysImageFolder, fileName);

                FileInfo file = new FileInfo(path);
                file.Directory.Create();

                File.WriteAllBytes(path, imageBuffer);
            }
            catch (Exception e)
            {
                logger.Error($"Failed to save image: {e.Message}");
            }
        }

        private static string GetAppDirectoryPath()
        {
            string appPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string appName = assemblyName.Substring(0, assemblyName.IndexOf('.'));

            return Path.Combine(appPath, appName);
        }
    }
}
