using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System;
using Avalonia.Threading;

namespace ProcessesMonitor;

public static class Utils
{
    public static async Task ShowMessageAsync(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ButtonDefinitions = ButtonEnum.Ok,
            ContentTitle = title,
            ContentMessage = message,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        });
        await box.ShowAsync();
    }

    public static async Task ShowErrorAsync(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(new MessageBoxStandardParams
        {
            ButtonDefinitions = ButtonEnum.Ok,
            ContentTitle = title,
            ContentMessage = message,
            Icon = Icon.Error,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        });
        await box.ShowAsync();
    }

    public static void HandleException(Exception? e)
    {
        if (e == null) return;

        // Log to console/debug output
        System.Diagnostics.Debug.WriteLine("An unhandled exception occurred:");
        System.Diagnostics.Debug.WriteLine(e.ToString());

        // Show a message box on the UI thread
        Dispatcher.UIThread.Post(async () =>
        {
            await ShowErrorAsync("Unhandled Exception", e.Message);
        });
    }
}