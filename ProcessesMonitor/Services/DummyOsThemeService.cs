namespace ProcessesMonitor.Services;

public class DummyOsThemeService : IOsThemeService
{
    public bool IsSupported => false;
    public void SetTheme(string theme)
    {
        // Do nothing
    }
}