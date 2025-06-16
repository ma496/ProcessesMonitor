using System.Runtime.Versioning;
using System.Security;
using Microsoft.Win32;

namespace ProcessesMonitor.Services;

[SupportedOSPlatform("windows")]
public class WindowsOsThemeService : IOsThemeService
{
    public bool IsSupported => true;
    private const string RegistryKeyPath = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
    private const string AppsUseLightTheme = "AppsUseLightTheme";
    private const string SystemUsesLightTheme = "SystemUsesLightTheme";

    public void SetTheme(string theme)
    {
        try
        {
            var value = theme == "Light" ? 1 : 0;
            Registry.SetValue(RegistryKeyPath, AppsUseLightTheme, value, RegistryValueKind.DWord);
            Registry.SetValue(RegistryKeyPath, SystemUsesLightTheme, value, RegistryValueKind.DWord);
        }
        catch (SecurityException)
        {
            // Handle insufficient permissions
        }
    }
} 