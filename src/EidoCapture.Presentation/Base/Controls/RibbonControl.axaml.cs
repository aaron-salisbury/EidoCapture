using Avalonia.Controls;
using EidoCapture.Presentation.ViewModels;
using System;

namespace EidoCapture.Presentation.Base.Controls
{
    public partial class RibbonControl : UserControl
    {
        public RibbonControl()
        {
            InitializeComponent();
            this.DataContext = App.Current?.Services?.GetService(typeof(ScreenCaptureViewModel));
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
