using Avalonia.Controls;
using Avalonia.Interactivity;
using EidoCapture.Presentation.Base.Extensions;
using System;

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

        private void DelaySecondsNumeric_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
        {
            if (e.NewValue != null && sender is NumericUpDown numeric)
            {
                numeric.Value = Math.Round(e.NewValue.Value, 0, MidpointRounding.AwayFromZero);
            }
        }
    }
}
