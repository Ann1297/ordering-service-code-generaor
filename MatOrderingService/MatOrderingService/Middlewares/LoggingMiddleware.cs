using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MatOrderingService.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly MyOptions _options;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, IOptions<MyOptions> options)
        {
            _logger = logger;
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation($"Let's handle {httpContext.Request.Path}...");
            _logger.LogInformation($"Here is my options: {_options.Option1} and {_options.Option2}");

            await _next(httpContext);

            _logger.LogInformation($"{httpContext.Request.Path} has been handled :)");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
