using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Plutus.Api.Middleware
{
    
    
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
            _logger.LogInformation(
                "[{@ApplicationName}-{@EnvironmentName}] - {@CurrentDateTime} |  {@StatusCode}  |" + "  {@ElapsedTime} ms".PadLeft(10, ' ')  + " |" +
                "  {@Method}".PadRight(16, ' ') +
                "{@Url}",
                _hostEnvironment.ApplicationName, _hostEnvironment.EnvironmentName, DateTime.Now.ToString("t"),
                context.Response.StatusCode, stopwatch.ElapsedMilliseconds, context.Request.Method,
                context.Request.GetDisplayUrl());
        }
    }
}