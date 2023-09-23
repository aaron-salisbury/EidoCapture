using EidoCapture.Business.Base;
using EidoCapture.Business.Services;
using EidoCapture.Data;
using EidoCapture.Domains;
using System.Globalization;

namespace EidoCapture.Business.Models
{
    public class ScreenCapturer : ObservableModel
    {
        public ICapturer Capturer { get; set; }

        private Action<byte[]>? _consumerScreenShotAction;

        public ScreenCapturer(Action<byte[]>? screenShotAction = null)
        {
            Capturer = new SCDotNetCapturer(SaveScreenShot);

            _consumerScreenShotAction = screenShotAction;
        }

        private void SaveScreenShot(byte[] imageBuffer)
        {
            CRUD.CreateImage(CreateImage(imageBuffer));

            if (_consumerScreenShotAction != null)
            {
                _consumerScreenShotAction.Invoke(imageBuffer);
            }
        }

        private static ScreenShotImage CreateImage(byte[] imageBuffer)
        {
            DateTime currentDate = DateTime.Now;
            string thisDaysImageFolder = currentDate.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            string fileName = currentDate.ToString("yyyy-MM-dd HH-mm-ss", DateTimeFormatInfo.InvariantInfo) + ".jpg";

            return new ScreenShotImage(thisDaysImageFolder, fileName, imageBuffer);
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
