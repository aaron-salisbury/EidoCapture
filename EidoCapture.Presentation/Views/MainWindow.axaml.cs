﻿using Avalonia.Controls;
using System.Runtime.InteropServices;

namespace EidoCapture.Presentation.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        SetTitleBarAndContentGridRows();
    }

    private void SetTitleBarAndContentGridRows()
    {
        RowDefinition titleBarRow = new() { Height = new GridLength(GetTitleBarHeight()) };
        RowDefinition contentRow = new() { Height = GridLength.Star };

        TitleBarAndContentGrid.RowDefinitions.Add(titleBarRow);
        TitleBarAndContentGrid.RowDefinitions.Add(contentRow);
    }

    public static double GetTitleBarHeight()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 0.0D : 30.0D;
    }
}
