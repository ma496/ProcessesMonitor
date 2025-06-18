using System.Diagnostics;
using System.Runtime.Versioning;

namespace ProcessesMonitor.Services;

[SupportedOSPlatform("linux")]
public class LinuxOsThemeService : IOsThemeService
{
    public bool IsSupported => true;

    public void SetTheme(string theme)
    {
        var themeName = theme == "Light" ? "Adwaita" : "Adwaita-dark";
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "gsettings",
                Arguments = $"set org.gnome.desktop.interface gtk-theme '{themeName}'",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        process.WaitForExit();
    }

    public bool IsLightTheme()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "gsettings",
                Arguments = "get org.gnome.desktop.interface gtk-theme",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return !output.ToLower().Contains("dark");
    }
} 