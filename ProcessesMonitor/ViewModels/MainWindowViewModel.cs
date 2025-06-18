using ProcessesMonitor.Services;

namespace ProcessesMonitor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ProcessesMonitorViewModel ProcessesMonitor { get; }
    public SettingsViewModel Settings { get; }
    public AboutViewModel About { get; }

    public MainWindowViewModel(ProcessesMonitorViewModel processesMonitor, SettingsViewModel settings, AboutViewModel about,
        ThemeService themeService)
    {
        ProcessesMonitor = processesMonitor;
        Settings = settings;
        About = about;
        var theme = themeService.LoadTheme();
        themeService.ApplyTheme(theme);
    }
}