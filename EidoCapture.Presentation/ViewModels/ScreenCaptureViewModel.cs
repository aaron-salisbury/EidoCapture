using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EidoCapture.Business.Models;
using EidoCapture.Presentation.Base;
using System.Collections.Generic;
using System.Linq;

namespace EidoCapture.Presentation.ViewModels;

public partial class ScreenCaptureViewModel : BaseViewModel
{
    public RelayCommand ActivateCaptureCommand { get; }
    public RelayCommand DeactivateCaptureCommand { get; }

    [ObservableProperty]
    private int _delaySeconds;

    [ObservableProperty]
    private string _selectedDisplay;

    [ObservableProperty]
    private List<string> _displays;

    [ObservableProperty]
    private bool _isCapturing;

    [ObservableProperty]
    private byte[]? _frameBuffer;

    private ScreenCapturer _screenCapturer;

    public ScreenCaptureViewModel()
    {
        _screenCapturer = new ScreenCapturer(UpdateFrameBuffer);
        _delaySeconds = 3;
        _displays = _screenCapturer.Capturer.GetDisplayNames().ToList();
        _selectedDisplay = _displays.FirstOrDefault() ?? string.Empty;

        ActivateCaptureCommand = new RelayCommand(() => OnActivateCapture());
        DeactivateCaptureCommand = new RelayCommand(() => OnDeactivateCapture());
    }

    public override void RemoveModelEvents()
    {
        _screenCapturer.Shutdown();
    }

    private void OnActivateCapture()
    {
        IsCapturing = true;

        _screenCapturer.Capturer.Activate(DelaySeconds, SelectedDisplay);
    }

    private void OnDeactivateCapture()
    {
        _screenCapturer.Capturer.Deactivate();

        FrameBuffer = null;
        IsCapturing = false;
    }

    private void UpdateFrameBuffer(byte[] buffer)
    {
        FrameBuffer = null; // I don't know why, but have to null-out before it will update with a new one after the first time.
        FrameBuffer = buffer;
    }
}
