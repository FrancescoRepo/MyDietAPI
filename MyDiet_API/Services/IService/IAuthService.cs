using Microsoft.AspNetCore.Identity;
using MyDiet_API.Shared.Auth;
using MyDiet_API.Shared.Dtos;
using System.Threading.Tasks;

namespace MyDiet_API.Services.IService
{
    public interface IAuthService
    {
        public Task<UserManagerResponse> LoginAsync(LoginDto loginDto);

        public Task<UserManagerResponse> RegisterAsync(RegisterDto registerDto);

        public Task<IdentityUser> GetUser(string userId);

        public LoginDto GetCredentials(string base64);
    }
}
