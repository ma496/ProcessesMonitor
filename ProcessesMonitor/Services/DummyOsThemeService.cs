using System.Threading.Tasks;

namespace ProcessesMonitor.Services;

public class DummyOsThemeService : IOsThemeService
{
    public bool IsSupported => false;
    public Task SetThemeAsync(string theme)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> IsLightThemeAsync()
    {
        throw new System.NotImplementedException();
    }
}