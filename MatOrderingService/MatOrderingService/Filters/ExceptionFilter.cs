using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatOrderingService.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger) : base()
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is Exception)
            {
                _logger.LogInformation($"Exception: {context.Exception.GetType()} Message: {context.Exception.Message}");
                if (context.Exception.InnerException != null)
                    _logger.LogInformation($"Inner Exception:{ context.Exception.InnerException.GetType()} { context.Exception.InnerException.Message}");
                context.Result = new BadRequestResult();
                context.ExceptionHandled = true;
            }
        }
    }
}
