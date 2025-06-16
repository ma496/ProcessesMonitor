using System.Runtime.InteropServices;
using ProcessesMonitor.Services;

namespace ProcessesMonitor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ProcessesMonitorViewModel ProcessesMonitor { get; }
    public SettingsViewModel Settings { get; }

    public MainWindowViewModel()
    {
        ProcessesMonitor = new ProcessesMonitorViewModel();
        
        IOsThemeService osThemeService;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            osThemeService = new WindowsOsThemeService();
        }
        else
        {
            // Placeholder for other OSes
            osThemeService = new DummyOsThemeService();
        }

        Settings = new SettingsViewModel(osThemeService);
    }
}