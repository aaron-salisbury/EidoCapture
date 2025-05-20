using System;

namespace EidoCapture.Business.Services
{
    public interface ICapturer
    {
        IEnumerable<string> GetDisplayNames();

        void Activate(decimal delaySeconds = 1.0M, string? displayName = null);

        void Deactivate();
    }
}
