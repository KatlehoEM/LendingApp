using System;

namespace API.CustomLoggers;
public class FileLogger : ILogger
{
    private readonly string _filePath;
    private readonly string _categoryName;
    private static readonly object _lock = new object();

    public FileLogger(string categoryName, string filePath)
    {
        _filePath = filePath;
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Information; // Set the minimum log level
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        var logRecord = $"{message}";

        lock (_lock)
        {
            File.AppendAllText(_filePath, logRecord + Environment.NewLine);
        }
    }
}
