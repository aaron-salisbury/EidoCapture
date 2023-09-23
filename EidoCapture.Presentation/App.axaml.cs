﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using EidoCapture.Presentation.ViewModels;
using EidoCapture.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace EidoCapture.Presentation;

public partial class App : Application
{
    public new static App? Current => Application.Current as App;

    public IServiceProvider? Services { get; set; }

    public override void Initialize()
    {
        Services = ConfigureServices();

        AvaloniaXamlLoader.Load(this);
    }

    private static IServiceProvider ConfigureServices()
    {
        // https://docs.microsoft.com/en-us/windows/communitytoolkit/mvvm/ioc
        ServiceCollection services = new();

        // This app requires the naming convention that views end in "View",
        // view models end in "ViewModel", and that nothing else ends in either.
        // See Base.ViewLocator.cs and Base.Extensions.UserControlExtensions.SetDataContext().
        foreach (Type appType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (appType.Name.EndsWith("ViewModel"))
            {
                services.AddSingleton(appType);
            }
        }

        return services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
        {
            singleView.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
