using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EidoCapture.Business.Models;
using EidoCapture.Presentation.Base;
using System.Collections.Generic;
using System.ComponentModel;
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
    private bool _showScreenShots;

    [ObservableProperty]
    private bool _isCapturing;

    [ObservableProperty]
    private byte[]? _frameBuffer;

    private ScreenCapturer _screenCapturer;

    public ScreenCaptureViewModel()
    {
        _screenCapturer = new ScreenCapturer();
        _delaySeconds = 3;
        _displays = _screenCapturer.Capturer.GetDisplayNames().ToList();
        _selectedDisplay = _displays.FirstOrDefault() ?? string.Empty;

        ActivateCaptureCommand = new RelayCommand(() => OnActivateCapture());
        DeactivateCaptureCommand = new RelayCommand(() => OnDeactivateCapture());
    }

    public override void AddModelEvents()
    {
        _screenCapturer.PropertyChanged += ScreenCapturer_PropertyChanged;
    }

    public override void RemoveModelEvents()
    {
        _screenCapturer.PropertyChanged -= ScreenCapturer_PropertyChanged;
        _screenCapturer.Shutdown();
    }

    private void ScreenCapturer_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is ScreenCapturer screenCapturer && nameof(ScreenCapturer.MostRecentScreenShotBuffer).Equals(e.PropertyName))
        {
            FrameBuffer = screenCapturer.MostRecentScreenShotBuffer;
        }
    }

    private void OnActivateCapture()
    {
        IsCapturing = true;

        _screenCapturer.AllocateNewImage = ShowScreenShots;
        _screenCapturer.Capturer.Activate(DelaySeconds, SelectedDisplay);
    }

    private void OnDeactivateCapture()
    {
        _screenCapturer.Capturer.Deactivate();

        FrameBuffer = null;

        IsCapturing = false;
    }
}
