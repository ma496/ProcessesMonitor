using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using Avalonia.Collections;

namespace ProcessesMonitor.ViewModels;

public partial class ProcessesMonitorViewModel : ViewModelBase, IDisposable
{
    private readonly DispatcherTimer _timer;
    private readonly Dictionary<int, (TimeSpan, DateTime)> _cpuUsageCache = [];
    private readonly long _totalPhysicalMemory;

    private readonly ObservableCollection<ProcessInfo> _processesCollection = [];
    public DataGridCollectionView Processes { get; }

    [ObservableProperty]
    private double _totalCpuUsagePercentage;

    [ObservableProperty]
    private double _totalMemoryUsagePercentage;

    private readonly Random _random = new();

    [ObservableProperty]
    private int _interval;

    public ProcessesMonitorViewModel()
    {
        Processes = new DataGridCollectionView(_processesCollection);
        _totalPhysicalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;

        LoadProcesses();

        Interval = _random.Next(3, 6);
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(Interval)
        };
        _timer.Tick += (_, _) => LoadProcesses();
        _timer.Start();
    }

    private void LoadProcesses()
    {
        try
        {
            var processList = Process.GetProcesses();
            var now = DateTime.Now;

            var newProcesses = new List<ProcessInfo>(processList.Length);
            long totalMemoryUsageBytes = 0;
            double cumulativeCpuUsage = 0;

            foreach (var process in processList)
            {
                try
                {
                    // calculate cpu usage
                    double cpu = 0.0;
                    if (_cpuUsageCache.TryGetValue(process.Id, out var previous))
                    {
                        var (lastTime, lastCheckTime) = previous;
                        var currentTime = process.TotalProcessorTime;
                        var timeSinceLastCheck = now - lastCheckTime;

                        if (timeSinceLastCheck.TotalMilliseconds > 0)
                        {
                            var cpuMillis = (currentTime - lastTime).TotalMilliseconds;
                            cpu = (cpuMillis / (Environment.ProcessorCount * timeSinceLastCheck.TotalMilliseconds)) * 100.0;
                        }
                    }

                    // Get memory in bytes for accurate total calculation
                    long memoryUsageBytes = process.WorkingSet64;
                    totalMemoryUsageBytes += memoryUsageBytes;
                    cumulativeCpuUsage += cpu;

                    newProcesses.Add(new ProcessInfo
                    {
                        Pid = process.Id,
                        Name = process.ProcessName,
                        // Convert to MB before storing in the ProcessInfo object
                        MemoryUsage = memoryUsageBytes / (1024.0 * 1024.0),
                        CpuUsage = cpu
                    });

                    _cpuUsageCache[process.Id] = (process.TotalProcessorTime, now);
                }
                catch (Exception)
                {
                    _cpuUsageCache.Remove(process.Id);
                }
            }

            // remove cpu usage cache for processes that are no longer running
            var runningPids = new HashSet<int>(newProcesses.Select(p => p.Pid));
            var exitedPids = _cpuUsageCache.Keys.Where(pid => !runningPids.Contains(pid)).ToList();
            foreach (var pid in exitedPids)
            {
                _cpuUsageCache.Remove(pid);
            }

            Dispatcher.UIThread.Post(() =>
            {
                // Calculate total usage percentages
                TotalCpuUsagePercentage = cumulativeCpuUsage;
                TotalCpuUsagePercentage = Math.Min(100, TotalCpuUsagePercentage);
                TotalMemoryUsagePercentage = _totalPhysicalMemory > 0
                    ? (double)totalMemoryUsageBytes / _totalPhysicalMemory * 100.0
                    : 0;

                _processesCollection.Clear();
                foreach (var process in newProcesses)
                {
                    _processesCollection.Add(process);
                }
            });
        }
        catch (Exception e)
        {
            Debug.WriteLine($"An error occurred while loading processes: {e}");
        }
    }

    [RelayCommand]
    private async Task ExportToCsv()
    {
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var path = Path.Combine(desktop, "processes.csv");
        await using var writer = new StreamWriter(path);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        var records = _processesCollection.Select(p => new
        {
            p.Pid,
            p.Name,
            CpuUsage = p.CpuUsageDisplay,
            MemoryUsage = p.MemoryUsageDisplay
        });

        await csv.WriteRecordsAsync(records);
    }

    public void Dispose()
    {
        _timer.Stop();
        _processesCollection.Clear();
        _cpuUsageCache.Clear();
        GC.SuppressFinalize(this);
    }
}

public partial class ProcessInfo : ViewModelBase
{
    [ObservableProperty]
    private int _pid;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MemoryUsageDisplay))]
    private double _memoryUsage; // in MB

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CpuUsageDisplay))]
    private double _cpuUsage; // as a percentage

    public string CpuUsageDisplay => $"{CpuUsage:F2} %";

    public string MemoryUsageDisplay => $"{MemoryUsage:F2} MB";
}