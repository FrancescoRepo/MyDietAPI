using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace MyDiet_API.Filters
{
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionHandlingFilter> _logger;
        private const string ERROR_MESSAGE = "Internal Server Error";

        public ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, $"Error occurred in {context.HttpContext.Request.Path}\n");
            SetExceptionResult(context, context.Exception, HttpStatusCode.InternalServerError);

            context.ExceptionHandled = true;
            string path = context.HttpContext.Request.Path;
            _logger.LogInformation("<<<<<<<<<<<< End of request {}", path);
        }

        private static void SetExceptionResult(
            ExceptionContext context,
            Exception exception,
            HttpStatusCode code)
        {
            context.Result = new JsonResult(new
            {
                StatusCode = code,
                Message = ERROR_MESSAGE
            })
            { 
                StatusCode = (int)code
            };
        }
    }
}
