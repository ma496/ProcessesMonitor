using System.Diagnostics;
using System.Security;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace ProcessesMonitor.Services;

[SupportedOSPlatform("macos")]
public class MacOsThemeService : IOsThemeService
{
    public bool IsSupported => true;

    public async Task SetThemeAsync(string theme)
    {
        try
        {
            var isLight = theme == "Light";
            var script = $"-e 'tell app \"System Events\" to tell appearance preferences to set dark mode to {(!isLight).ToString().ToLower()}'";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "osascript",
                    Arguments = script,
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
            await Utils.ShowErrorAsync("Error", $"An security error occurred while setting the Mac OS theme: {ex}");
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
                    FileName = "osascript",
                    Arguments = "-e 'tell app \"System Events\" to tell appearance preferences to get dark mode'",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output.Trim() == "false";
        }
        catch (SecurityException ex)
        {
            await Utils.ShowErrorAsync("Error", $"An security error occurred while checking the Mac OS theme: {ex}");
        }

        return true;
    }
}