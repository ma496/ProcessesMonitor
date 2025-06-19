using System.Threading.Tasks;

namespace ProcessesMonitor.Services;

public interface IOsThemeService
{
    bool IsSupported { get; }
    Task SetThemeAsync(string theme);
    Task<bool> IsLightThemeAsync();
} 