using Avalonia.Controls;
using Avalonia.Interactivity;
using EidoCapture.Presentation.Base.Extensions;

namespace EidoCapture.Presentation.Views
{
    public partial class ScreenCaptureView : UserControl
    {
        public ScreenCaptureView()
        {
            InitializeComponent();
            this.SetDataContext(App.Current?.Services);
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            this.LoadModelEvents();
        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {
            base.OnUnloaded(e);

            this.UnloadModelEvents();
        }
    }
}
