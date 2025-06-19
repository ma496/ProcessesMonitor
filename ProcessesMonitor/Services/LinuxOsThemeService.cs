using System.Diagnostics;
using System.Security;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace ProcessesMonitor.Services;

[SupportedOSPlatform("linux")]
public class LinuxOsThemeService : IOsThemeService
{
    public bool IsSupported => true;

    public async Task SetThemeAsync(string theme)
    {
        try
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
        catch (SecurityException ex)
        {
            await Utils.ShowErrorAsync("Error", $"An security error occurred while setting the Linux OS theme: {ex}");
        }
    }

    public async Task<bool> IsLightThemeAsync()
    {
        try
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
        catch (SecurityException ex)
        {
            await Utils.ShowErrorAsync("Error", $"An security error occurred while checking the Linux OS theme: {ex}");
        }

        return true;
    }
}