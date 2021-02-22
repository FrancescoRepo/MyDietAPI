using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyDiet_API.Services.IService;
using MyDiet_API.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiet_API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext, IAuthTokenService tokenService, IAuthService authService)
        {
            var routePath = httpContext.Request.Path;
            _logger.LogInformation("Entered in AuthMiddleware with routePath {}", routePath);

            string errorResponse;

            if (!routePath.Value.Contains(Constants.REGISTER) && !routePath.Value.Contains(Constants.LOGIN))
            {
                if (httpContext.Request.Headers.ContainsKey(Constants.AUTHORIZATION_HEADER))
                {
                    var authHeader = httpContext.Request.Headers[Constants.AUTHORIZATION_HEADER];
                    if (!string.IsNullOrEmpty(authHeader))
                    {
                        if (authHeader.FirstOrDefault().StartsWith($"{Constants.BEARER_AUTHORIZATION_KEYWORD}"))
                        {
                            string token = authHeader.FirstOrDefault().Split(Constants.BEARER_AUTHORIZATION_KEYWORD)[1].Trim();
                            bool isValidToken = tokenService.ValidateToken(token);

                            if (isValidToken)
                            {
                                string userId = tokenService.GetUserFromToken(token);
                                if (userId != null)
                                {
                                    var user = await authService.GetUser(userId);
                                    if (user != null)
                                        await _next(httpContext);
                                    else
                                    {
                                        _logger.LogError(Constants.TOKEN_NOT_VALID);
                                        errorResponse = Utilities.HttpResponse.CreateErrorResponse(401, Constants.TOKEN_NOT_VALID, ref httpContext, Constants.APPLICATION_JSON_CONTENT_TYPE);
                                        await httpContext.Response.WriteAsync(errorResponse);
                                    }
                                }
                                else
                                {
                                    _logger.LogError(Constants.TOKEN_NOT_VALID);
                                    errorResponse = Utilities.HttpResponse.CreateErrorResponse(401, Constants.TOKEN_NOT_VALID, ref httpContext, Constants.APPLICATION_JSON_CONTENT_TYPE);
                                    await httpContext.Response.WriteAsync(errorResponse);
                                }
                            }
                            else
                            {
                                _logger.LogError(Constants.TOKEN_NOT_VALID);
                                errorResponse = Utilities.HttpResponse.CreateErrorResponse(401, Constants.TOKEN_NOT_VALID, ref httpContext, Constants.APPLICATION_JSON_CONTENT_TYPE);
                                await httpContext.Response.WriteAsync(errorResponse);
                            }
                        }
                        else
                        {
                            _logger.LogError(Constants.INVALID_FORMAT_AUTHORIZATION_HEADER);
                            errorResponse = Utilities.HttpResponse.CreateErrorResponse(401, Constants.INVALID_FORMAT_AUTHORIZATION_HEADER, ref httpContext, Constants.APPLICATION_JSON_CONTENT_TYPE);
                            await httpContext.Response.WriteAsync(errorResponse);
                        }
                    }
                }
                else
                {
                    _logger.LogError(Constants.NO_AUTHORIZATION_HEADER);
                    errorResponse = Utilities.HttpResponse.CreateErrorResponse(401, Constants.NO_AUTHORIZATION_HEADER, ref httpContext, Constants.APPLICATION_JSON_CONTENT_TYPE);
                    await httpContext.Response.WriteAsync(errorResponse);
                }
            }
            else
                await _next(httpContext);
        }
    }

    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
