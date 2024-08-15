using System.Diagnostics;

namespace Prueba.API.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Handling request: {Method} {Url}", context.Request.Method, context.Request.Path);

        var stopwatch = Stopwatch.StartNew();
        await _next(context);
        stopwatch.Stop();

        _logger.LogInformation("Finished handling request. Status code: {StatusCode}. Time taken: {ElapsedMilliseconds}ms", context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}