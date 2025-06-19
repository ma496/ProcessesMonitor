using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using ProcessesMonitor.ViewModels;
using ProcessesMonitor.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using ProcessesMonitor.Services;
using System.Runtime.InteropServices;
using Avalonia.Threading;

namespace ProcessesMonitor;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public new static App Current => (App)Application.Current!;

    public IServiceProvider ServiceProvider { get; } = ConfigureServices();

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // configure services
        services.AddSingleton<ThemeService>();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            services.AddSingleton<IOsThemeService, WindowsOsThemeService>();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            services.AddSingleton<IOsThemeService, MacOsThemeService>();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            services.AddSingleton<IOsThemeService, LinuxOsThemeService>();
        }
        else
        {
            services.AddSingleton<IOsThemeService, DummyOsThemeService>();
        }

        // configure view models
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<ProcessesMonitorViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<AboutViewModel>();

        return services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            Dispatcher.UIThread.UnhandledException += (_, e) =>
            {
                // Mark as handled to prevent the AppDomain handler from firing.
                e.Handled = true;
                Utils.HandleException(e.Exception);
            };
            desktop.MainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}