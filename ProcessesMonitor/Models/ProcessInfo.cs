namespace ProcessesMonitor.Models;

public class ProcessInfo
{
    public int Pid { get; set; }
    public string? Name { get; set; }
    public double CpuUsage { get; set; }
    public double MemoryUsage { get; set; }
} 