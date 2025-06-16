using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Styling;

namespace ProcessesMonitor.Services;

public class ThemeService
{
    private const string SettingsFile = "settings.json";

    public void ApplyTheme(string theme)
    {
        var app = Application.Current;
        if (app != null)
        {
            app.RequestedThemeVariant = theme == "Dark" ? ThemeVariant.Dark 
                : theme == "Light" ? ThemeVariant.Light : ThemeVariant.Default;
        }
    }

    public void SetTheme(string theme)
    {
        ApplyTheme(theme);
        SaveTheme(theme);
    }

    public string LoadTheme()
    {
        if (File.Exists(SettingsFile))
        {
            var json = File.ReadAllText(SettingsFile);
            var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (settings != null && settings.TryGetValue("Theme", out var theme))
            {
                return theme;
            }
        }
        return "System"; // Default theme
    }

    private void SaveTheme(string theme)
    {
        var settings = new Dictionary<string, string> { { "Theme", theme } };
        var json = JsonSerializer.Serialize(settings);
        if (!File.Exists(SettingsFile))
        {
            File.Create(SettingsFile).Close();
        }
        File.WriteAllText(SettingsFile, json);
    }
} 