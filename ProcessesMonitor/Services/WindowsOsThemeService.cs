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

    public bool IsLightTheme()
    {
        try
        {
            // Get the value of the AppsUseLightTheme registry key.
            // The second argument is the default value to return if the key is not found.
            var registryValueObject = Registry.GetValue(RegistryKeyPath, AppsUseLightTheme, 1);

            // Check if the retrieved value is not null and is a non-zero integer.
            // In the Windows registry, a value of 1 for AppsUseLightTheme indicates that the light theme is enabled.
            if (registryValueObject is int registryValue)
            {
                return registryValue == 1;
            }
        }
        catch (SecurityException)
        {
            // Handle insufficient permissions
        }
        
        // Default to light theme if the key is not found or in case of an error.
        return true;
    }
} 