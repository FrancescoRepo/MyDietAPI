using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Auth;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.Utilities;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace MyDiet_API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a User", Description = "Create a new User")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            _logger.LogInformation("Entered in /register endpoint with registerDto {}", registerDto);
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(registerDto);
                if (result.IsSuccess) return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Register properties not valid");
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login a User", Description = "Login a User")]
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> Login()
        {
            _logger.LogInformation("Entered in /login endpoint");
            if (HttpContext.Request.Headers.ContainsKey(Constants.AUTHORIZATION_HEADER))
            {
                string base64 = HttpContext.Request.Headers[Constants.AUTHORIZATION_HEADER];
                LoginDto loginDto = _authService.GetCredentials(base64);

                if (loginDto != null)
                {
                    var result = await _authService.LoginAsync(loginDto);
                    if (result.IsSuccess) return Ok(result);
                    else return BadRequest("Error while login the user");
                }
                else
                    return BadRequest(new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Error while decoding auth"
                    });
            }

            return BadRequest("No Authorization Header found");
        }
    }
}
