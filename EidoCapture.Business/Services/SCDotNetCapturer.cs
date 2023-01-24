using Microsoft.IO;
using ScreenCapture.NET;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EidoCapture.Business.Services
{
    public class SCDotNetCapturer : ICapturer
    {
        public bool IsCaptureRunning
        {  
            get { return _captureTask != null && _captureTask.Status == TaskStatus.Running; }
        }

        private static readonly RecyclableMemoryStreamManager _recyclableStreamManager = new RecyclableMemoryStreamManager();

        private Action<byte[]> _imageCapturedAction;
        private CancellationTokenSource _captureTokenSource;
        private Task _captureTask;

        public SCDotNetCapturer(Action<byte[]> imageCapturedAction)
        {
            _imageCapturedAction = imageCapturedAction;
        }

        public IEnumerable<string> GetDisplayNames()
        {
            IScreenCaptureService screenCaptureService = new DX11ScreenCaptureService();
            IEnumerable<GraphicsCard> graphicsCards = screenCaptureService.GetGraphicsCards();
            IEnumerable<Display> displays = screenCaptureService.GetDisplays(graphicsCards.First());

            return displays.Select(d => d.DeviceName.Replace(".", string.Empty).Replace("\\", string.Empty));
        }

        public void Activate(decimal delaySeconds = 1.0M, string displayName = null)
        {
            Manager.Logger.Information("Beginning to capture screenshots.");

            Deactivate();

            delaySeconds = delaySeconds < 1.0M ? 1.0M : delaySeconds;
            int captureDelayMilliseconds = Convert.ToInt32(Math.Round(delaySeconds, 2, MidpointRounding.AwayFromZero) * 1000);

            _captureTokenSource = new CancellationTokenSource();
            _captureTask = Task.Run(() => Capture(displayName, captureDelayMilliseconds, _captureTokenSource.Token));
        }

        public void Deactivate()
        {
            if (IsCaptureRunning)
            {
                _captureTokenSource.Cancel();
                _captureTask.Wait();

                _captureTokenSource.Dispose();
                _captureTask.Dispose();

                _captureTask = null;

                Manager.Logger.Information("Ended screen capturing.");
            }
        }

        private void Capture(string captureDisplayName, int captureDelayMilliseconds, CancellationToken cancelToken)
        {
            IScreenCaptureService screenCaptureService = new DX11ScreenCaptureService();
            IEnumerable<GraphicsCard> graphicsCards = screenCaptureService.GetGraphicsCards();
            IEnumerable<Display> displays = screenCaptureService.GetDisplays(graphicsCards.First());

            Display captureDisplay = !string.IsNullOrEmpty(captureDisplayName)
                ? displays.Where(d => d.DeviceName.Contains(captureDisplayName)).First()
                : displays.First();

            using (IScreenCapture screenCapture = screenCaptureService.GetScreenCapture(captureDisplay))
            {
                CaptureZone captureZone = screenCapture.RegisterCaptureZone(0, 0, screenCapture.Display.Width, screenCapture.Display.Height);

                while (!cancelToken.IsCancellationRequested)
                {
                    _imageCapturedAction(GetScreenshotImage(screenCapture, captureZone).Result);

                    Thread.Sleep(captureDelayMilliseconds);
                }
            }
        }

        private static Task<byte[]> GetScreenshotImage(IScreenCapture screenCapture, CaptureZone captureZone)
        {
            return Task.Run(() =>
            {
                screenCapture.CaptureScreen();

                lock (captureZone.Buffer)
                {
                    using Image image = Image.LoadPixelData<Bgra32>(captureZone.Buffer, captureZone.Width, captureZone.Height);
                    using MemoryStream jpgStream = _recyclableStreamManager.GetStream();
                    image.SaveAsJpeg(jpgStream);

                    return jpgStream.GetBuffer();
                }
            });
        }
    }
}
