using Avalonia;
using Avalonia.Controls;

namespace EidoCapture.Presentation.Base.Controls
{
    public partial class ViewHeaderControl : UserControl
    {
        public static readonly DirectProperty<ViewHeaderControl, string> FriendlyAppNameProperty =
            AvaloniaProperty.RegisterDirect<ViewHeaderControl, string>(nameof(FriendlyAppName), o => o.FriendlyAppName, (o, v) => o.FriendlyAppName = v);

        private string _friendlyAppName = "PLACEHOLDER";
        public string FriendlyAppName
        {
            get { return _friendlyAppName; }
            set { SetAndRaise(FriendlyAppNameProperty, ref _friendlyAppName, value); }
        }

        public ViewHeaderControl()
        {
            InitializeComponent();
        }
    }
}
