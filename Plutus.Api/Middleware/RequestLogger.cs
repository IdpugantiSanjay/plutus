using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;

namespace Plutus.Api.Middleware;


public class RequestLogger
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLogger> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public RequestLogger(RequestDelegate next, ILogger<RequestLogger> logger, IHostEnvironment hostEnvironment)
    {
        _next = next;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await _next(context);
        stopwatch.Stop();

        const string TEMPLATE = @"
            [{@ApplicationName}-{@EnvironmentName}] - {@CurrentDateTime} |  {@StatusCode}  |  {@ElapsedTime} ms | {@Method} {@Url}
        ";

        _logger.LogInformation(
            TEMPLATE,
            _hostEnvironment.ApplicationName,
            _hostEnvironment.EnvironmentName,
            DateTime.Now.ToString("t"),
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds.ToString().PadLeft(10, ' '),
            context.Request.Method.PadRight(16, ' '),
            context.Request.GetDisplayUrl()
         );
    }
}
