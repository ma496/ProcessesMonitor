using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProcessesMonitor.Services;

namespace ProcessesMonitor.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ThemeService _themeService;
    public IOsThemeService OsThemeService { get; }

    [ObservableProperty]
    private string _selectedTheme;

    public SettingsViewModel(IOsThemeService osThemeService)
    {
        _themeService = new ThemeService(osThemeService);
        OsThemeService = osThemeService;
        _selectedTheme = _themeService.LoadTheme();
    }
    
    public bool IsLight => SelectedTheme == "Light";

    public bool IsDark => SelectedTheme == "Dark";

    public bool IsSystem => SelectedTheme == "System";

    [RelayCommand]
    private void SetTheme(string theme)
    {
        _themeService.SetTheme(theme);
        SelectedTheme = theme;
    }

    [RelayCommand]
    private void SetOsTheme(string theme)
    {
        OsThemeService.SetTheme(theme);
        if (SelectedTheme == "System")
        {
            _themeService.ApplyTheme(theme);
        }
    }
} 