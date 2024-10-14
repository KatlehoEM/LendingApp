using System;

namespace API.CustomLoggers;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;

    public FileLoggerProvider(string filePath)
    {
         _filePath = filePath;

        // Ensure the directory exists
        var directory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(categoryName, _filePath);
    }

    public void Dispose()
    {
        // No resources to release in this case
    }
}


