using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using ProcessesMonitor.Models;

namespace ProcessesMonitor.ViewModels;

public partial class ProcessesMonitorViewModel : ViewModelBase, IDisposable
{
    [ObservableProperty]
    private ObservableCollection<ProcessInfo> _processes = [];

    public ProcessesMonitorViewModel()
    {
        _processes.Add(new ProcessInfo { Pid = 1, Name = "Test" });
        Debug.WriteLine($"Processes: {Processes.Count}");
        // Dispatcher.UIThread.Post(LoadProcesses);
    }

    // public void LoadProcesses()
    // {
    //     // To avoid blocking the UI, it's better to run this on a background thread
    //     // and then update the collection on the UI thread.
    //     Task.Run(() =>
    //     {
    //         var processList = Process.GetProcesses();
    //         Debug.WriteLine($"Found {processList.Length} processes");

    //         // Switch back to the UI thread to update the collection.
    //         Dispatcher.UIThread.InvokeAsync(() =>
    //         {
    //             Processes.Clear(); // Clear previous list
    //             foreach (var process in processList)
    //             {
    //                 Processes.Add(new ProcessInfo { Pid = process.Id, Name = process.ProcessName });
    //             }
    //         });
    //     });
    // }

    [RelayCommand]
    private void ExportToCsv()
    {
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var path = Path.Combine(desktop, "processes.csv");
        using var writer = new StreamWriter(path);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(Processes);
    }

    public void Dispose()
    {
        Processes.Clear();
        Debug.WriteLine("ProcessMonitorViewModel disposed");
    }
}