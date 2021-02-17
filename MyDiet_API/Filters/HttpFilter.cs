using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MyDiet_API.Filters
{
    public class HttpFilter : IActionFilter
    {
        private readonly ILogger<HttpFilter> _logger;

        public HttpFilter(ILogger<HttpFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string path = context.HttpContext.Request.Path;
            if(context.Exception != null && context.ExceptionHandled)
                _logger.LogInformation("<<<<<<<<<<<< End of request {}", path);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string path = context.HttpContext.Request.Path;
            _logger.LogInformation(">>>>>>>>>>>> Accepted new request {}", path);
        }
    }
}
