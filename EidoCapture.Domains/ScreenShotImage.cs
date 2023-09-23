using System;

namespace EidoCapture.Domains
{
    [Serializable]
    public class ScreenShotImage
    {
        public string ParentDirectory { get; set; }
        public string FileName { get; set; }
        public byte[] Buffer { get; set; }

        public ScreenShotImage(string parentDirectory, string fileName, byte[] imageBuffer)
        {
            ParentDirectory = parentDirectory;
            FileName = fileName;
            Buffer = imageBuffer;
        }
    }
}
