using ProcessesMonitor.Services;

namespace ProcessesMonitor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ProcessesMonitorViewModel ProcessesMonitor { get; }
    public SettingsViewModel Settings { get; }

    public MainWindowViewModel(ProcessesMonitorViewModel processesMonitor, SettingsViewModel settings,
        ThemeService themeService)
    {
        ProcessesMonitor = processesMonitor;
        Settings = settings;
        var theme = themeService.LoadTheme();
        themeService.ApplyTheme(theme);
    }
}