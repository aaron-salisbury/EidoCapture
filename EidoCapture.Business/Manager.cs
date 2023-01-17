using EidoCapture.Business.Services;
using EidoCapture.Data;
using Serilog;
using System;

namespace EidoCapture.Business
{
    public class Manager
    {
        internal static ILogger Logger { get; private set; }

        public ICapturer Capturer { get; set; }

        public Manager(ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Capturer = new SCDotNetCapturer(SaveScreenshot);
        }

        private void SaveScreenshot(byte[] imageBuffer)
        {
            CRUD.CreateImage(imageBuffer, null, Logger);
        }

        /// <summary>
        /// Get the directory path where the application data gets saved.
        /// </summary>
        public string GetUserStorageDirectory()
        {
            return CRUD.AppDirectoryPath;
        }

        /// <summary>
        /// Release resources and update stored application data.
        /// </summary>
        public void Shutdown()
        {
            Logger.Information("Shutting down.");

            Capturer.Deactivate();
        }
    }
}
