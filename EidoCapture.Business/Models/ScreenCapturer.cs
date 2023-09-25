using EidoCapture.Business.Base;
using EidoCapture.Business.Services;
using EidoCapture.Data;

namespace EidoCapture.Business.Models
{
    public class ScreenCapturer : ObservableModel
    {
        public ICapturer Capturer { get; set; }

        private byte[]? _mostRecentScreenShotBuffer;
        public byte[]? MostRecentScreenShotBuffer
        {
            get => _mostRecentScreenShotBuffer;
            set => SetProperty(ref _mostRecentScreenShotBuffer, value);
        }

        public ScreenCapturer()
        {
            Capturer = new SCDotNetCapturer(SaveScreenShot);
        }

        private void SaveScreenShot(byte[] imageBuffer)
        {
            ScreenShotImage image = new(imageBuffer);

            CRUD.CreateImage(image.Buffer, image.ScopedFilePath());

            MostRecentScreenShotBuffer = image.Buffer;
        }

        /// <summary>
        /// Release resources.
        /// </summary>
        public void Shutdown()
        {
            Capturer.Deactivate();
        }
    }
}
