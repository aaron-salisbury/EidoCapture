using ScreenCapture.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
                    //TODO: Figure out cross-platform solution to convert buffer to jpeg.
                    // This uses System.Drawing.Common which since .NET 7, non-Windows platforms are not supported, even with the runtime configuration switch.
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        GCHandle pinnedArray = GCHandle.Alloc(captureZone.Buffer, GCHandleType.Pinned);
                        IntPtr pointer = pinnedArray.AddrOfPinnedObject();

                        Bitmap bitmap = new Bitmap(captureZone.Width, captureZone.Height, captureZone.Stride, PixelFormat.Format32bppArgb, pointer);

                        pinnedArray.Free();

                        using MemoryStream jpgStream = new MemoryStream();
                        bitmap.Save(jpgStream, ImageFormat.Jpeg);

                        return jpgStream.ToArray();
                    }

                    return captureZone.Buffer;
                }
            });
        }
    }
}
