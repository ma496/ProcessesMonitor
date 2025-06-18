using System.Diagnostics;
using System.Runtime.Versioning;

namespace ProcessesMonitor.Services;

[SupportedOSPlatform("macos")]
public class MacOsThemeService : IOsThemeService
{
    public bool IsSupported => true;

    public void SetTheme(string theme)
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

    public bool IsLightTheme()
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
} 