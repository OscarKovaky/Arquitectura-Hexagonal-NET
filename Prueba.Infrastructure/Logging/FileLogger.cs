using Microsoft.Extensions.Logging;

namespace Prueba.Infrastructure.Logging;

public class FileLogger : ILogger
{
    private readonly string _filePath;
    private static readonly object _lock = new object();

    public FileLogger(string filePath)
    {
        _filePath = filePath;
        
        // Crear el directorio si no existe
        var logDirectory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {formatter(state, exception)}";

        lock (_lock)
        {
            File.AppendAllText(_filePath, message + Environment.NewLine);
        }
    }
}