using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MyDiet_API.Utilities
{
    public static class HttpResponse
    {
        public static string CreateErrorResponse(int statusCode, string message)
        {
            return JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                Message = message
            });
        }

        public static string CreateErrorResponse(int statusCode, string message, ref HttpContext httpContext, string contentType = null)
        {
            httpContext.Response.StatusCode = statusCode;
            if (contentType != null)
                httpContext.Response.ContentType = contentType;

            return JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                Message = message
            });
        }
    }
}
