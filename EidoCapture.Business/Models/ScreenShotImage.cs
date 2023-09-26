using EidoCapture.Business.Base;
using System.Globalization;

namespace EidoCapture.Business.Models
{
    public class ScreenShotImage : ObservableModel
    {
        public string ParentDirectory { get; set; }
        public string FileName { get; set; }
        public byte[] Buffer { get; set; }

        public string ScopedFilePath () => Path.Combine(ParentDirectory, FileName);

        public ScreenShotImage(byte[] imageBuffer, bool allocateNewImage = false)
        {
            DateTime currentDate = DateTime.Now;
            string thisDaysImageFolder = currentDate.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            string fileName = currentDate.ToString("yyyy-MM-dd HH-mm-ss", DateTimeFormatInfo.InvariantInfo) + ".jpg";

            ParentDirectory = thisDaysImageFolder;
            FileName = fileName;

            if (allocateNewImage)
            {
                Buffer = new byte[imageBuffer.Length];
                imageBuffer.CopyTo(Buffer, 0);
            }
            else
            {
                Buffer = imageBuffer;
            }
        }
    }
}
