using Avalonia;
using Avalonia.Controls;

namespace EidoCapture.Presentation.Base.Controls
{
    public partial class ViewHeaderControl : UserControl
    {
        public static readonly DirectProperty<ViewHeaderControl, string> FriendlyPageNameProperty =
            AvaloniaProperty.RegisterDirect<ViewHeaderControl, string>(nameof(FriendlyPageName), o => o.FriendlyPageName, (o, v) => o.FriendlyPageName = v);

        public static readonly DirectProperty<ViewHeaderControl, object?> ButtonsContentProperty =
            AvaloniaProperty.RegisterDirect<ViewHeaderControl, object?>(nameof(ButtonsContent), o => o.ButtonsContent, (o, v) => o.ButtonsContent = v);

        private string _friendlyPageName = "PLACEHOLDER";
        public string FriendlyPageName
        {
            get { return _friendlyPageName; }
            set { SetAndRaise(FriendlyPageNameProperty, ref _friendlyPageName, value); }
        }

        private object? _buttonsContent;
        public object? ButtonsContent
        {
            get { return _buttonsContent; }
            set { SetAndRaise(ButtonsContentProperty, ref _buttonsContent, value); }
        }

        public ViewHeaderControl()
        {
            InitializeComponent();
        }
    }
}
