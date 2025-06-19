using Avalonia;
using System;
using System.Threading.Tasks;

namespace ProcessesMonitor;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // 1. AppDomain level: This is a last resort.
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            Utils.HandleException(e.ExceptionObject as Exception);

        // 2. TaskScheduler level: Catches unhandled exceptions from Tasks that are not awaited.
        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            Utils.HandleException(e.Exception);
            e.SetObserved();
        };

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}