namespace ProcessesMonitor.Services;

public interface IOsThemeService
{
    bool IsSupported { get; }
    void SetTheme(string theme);
    bool IsLightTheme();
} 