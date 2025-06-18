using CommunityToolkit.Mvvm.ComponentModel;
using ProcessesMonitor.ViewModels;

namespace ProcessesMonitor.ViewModels;

public partial class AboutViewModel : ViewModelBase
{
    public string Title => "About Processes Monitor";
    public string Version => "Version 1.0.0";
    public string Description => "A simple tool to monitor running processes.";
    public string Author => "Created by Muhammad Ali";
} 