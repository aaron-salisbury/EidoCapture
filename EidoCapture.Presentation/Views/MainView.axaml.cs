using Avalonia.Controls;
using Avalonia.Interactivity;
using EidoCapture.Presentation.Base.Extensions;

namespace EidoCapture.Presentation.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        this.SetDataContext(App.Current?.Services);
    }
}
