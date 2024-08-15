using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Prueba.Infrastructure.Logging;

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string filePath)
    {
        builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>(services => new FileLoggerProvider(filePath));
        return builder;
    }
}