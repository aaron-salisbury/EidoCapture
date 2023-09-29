using Avalonia;
using Avalonia.Controls;
using EidoCapture.Presentation.Base.Extensions;

namespace EidoCapture.Presentation.Views;

public partial class MainView : UserControl
{
    private const double SETTINGS_TAB_BOTTOM_MARGIN = 135.0D;

    TabItem? _settingsTabItem = null;
    private double _settingsMarginDif = 0.0D;

    public MainView()
    {
        InitializeComponent();
        this.SetDataContext(App.Current?.Services);

        this.SizeChanged += MainView_OnSizeChanged;
    }

    private void MainView_OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (_settingsTabItem == null)
        {
            _settingsTabItem = this.FindControl<TabItem>("SettingsTabItem");
            _settingsMarginDif = (_settingsTabItem?.Bounds.Top ?? 0.0D) + SETTINGS_TAB_BOTTOM_MARGIN - (_settingsTabItem?.Bounds.Height ?? 0.0D);
        }

        if (_settingsTabItem != null)
        {
            double marginTop = this.Bounds.Bottom - _settingsMarginDif;
            marginTop = marginTop >= 0.0D ? marginTop : 0.0D;

            _settingsTabItem.Margin = new Thickness(0, marginTop, 0, 0);
        }
    }
}
