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
                // A new reference is needed when the value is used in a binding.
                // But when done in a loop, this is a lot of allocations and
                // continuously spikes memory in-between GC collections.
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
