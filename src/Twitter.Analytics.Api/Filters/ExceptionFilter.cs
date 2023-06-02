using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Twitter.Analytics.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            var logId = Guid.NewGuid().ToString();

            var errorLog = new
            {
                Id = logId,
                Path = context.HttpContext.Request.Path,
                ErrorType = exception.GetType().Name,
                ErrorMessage = exception.Message,
                StackTrace = exception.StackTrace
            };

            // var options = new JsonSerializerOptions().Default();
            // var errorLogJson = JsonSerializer.Serialize(errorLog, options);

            // _logger.LogError(errorLogJson);

            context.Result = new JsonResult(new { logId })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
