using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MyDiet_API.Services.IService;
using MyDiet_API.Shared.Auth;
using MyDiet_API.Shared.Dtos;
using MyDiet_API.Utilities;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiet_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthTokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<IdentityUser> userManager, IMapper mapper, IAuthTokenService tokenService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<UserManagerResponse> RegisterAsync(RegisterDto registerDto)
        {
            _logger.LogInformation("Entered in RegisterAsync with registerDto {}", registerDto);

            if (registerDto == null)
                throw new NullReferenceException("Register Dto is null");

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                _logger.LogError("Password and ConfirmPassword doesn't match");
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };
            }

            IdentityUser user = new IdentityUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                _logger.LogDebug("User created [{}]", user);
                string token = _tokenService.GenerateToken(user);
                _logger.LogDebug("Token {}", token);

                return new UserManagerResponse
                {
                    Token = token,
                    IsSuccess = true,
                };
            }

            _logger.LogError("Error while creating user: {}", result.Errors.Select(e => e.Description));
            return new UserManagerResponse
            {
                Message = "Error during the registration of the user",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation("Entered in LoginAsync with loginDto {}", loginDto);

            if (loginDto == null)
                throw new NullReferenceException("Login Dto is null");

            _logger.LogInformation("Searching user with email {}", loginDto.Email);
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                _logger.LogError("User not found");
                return new UserManagerResponse
                {
                    Message = "No user found with the specified email address",
                    IsSuccess = false
                };
            }

            _logger.LogInformation("Checking user password");
            var passwordMatch = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!passwordMatch)
            {
                _logger.LogError("Invalid Password entered");

                return new UserManagerResponse
                {
                    Message = "Invalid Password",
                    IsSuccess = false
                };
            }

            _logger.LogDebug("User found: {}", user);
            string token = _tokenService.GenerateToken(user);
            _logger.LogDebug("Token {}", token);

            return new UserManagerResponse
            {
                Token = token,
                IsSuccess = true,
            };
        }

        public async Task<IdentityUser> GetUser(string userId)
        {
            _logger.LogDebug("Entered in GetUser with userId {}", userId);
            return await _userManager.FindByIdAsync(userId);
        }

        public LoginDto GetCredentials(string base64)
        {
            _logger.LogDebug("Entered in GetCredentials endpoint with base64 {}", base64);

            if (string.IsNullOrEmpty(base64))
            {
                _logger.LogError("base64 string is null or empty");
                return null;
            }
            
            byte[] data = Convert.FromBase64String(base64.Split(Constants.BASIC_AUTHORIZATION_KEYWORD)[1].Trim());
            string decodedString = Encoding.UTF8.GetString(data);

            if (!decodedString.Contains(":"))
            {
                _logger.LogError("Invalid base64 format string");
                return null;
            }
            string[] credentials = decodedString.Split(":");
            
            return new LoginDto(credentials[0].Trim(), credentials[1].Trim());
        }
    }
}
