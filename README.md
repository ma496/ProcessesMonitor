# Processes Monitor

A simple cross-platform application to monitor system processes. Built with Avalonia UI.

## Features

- View a list of running processes.
- See details for each process like PID, name, memory usage, and CPU usage.
- More to come.

## Build Instructions

To build this project, you will need the [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).

1.  Clone the repository:
    ```bash
    git clone https://github.com/your-username/ProcessesMonitor.git
    cd ProcessesMonitor
    ```

2.  Build the project:
    ```bash
    dotnet build -c Release
    ```

### Publishing for different platforms

You can create a self-contained application for your specific platform.

#### Windows
```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

#### macOS
```bash
dotnet publish -c Release -r osx-x64 --self-contained true
```

#### Linux
```bash
dotnet publish -c Release -r linux-x64 --self-contained true
```

The published application will be in the `bin/Release/net9.0/<runtime-identifier>/publish/` directory. 