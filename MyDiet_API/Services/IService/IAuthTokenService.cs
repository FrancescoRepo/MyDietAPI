using Microsoft.AspNetCore.Identity;

namespace MyDiet_API.Services.IService
{
    public interface IAuthTokenService
    {
        public string GenerateToken(IdentityUser user);
        public bool ValidateToken(string token);
        public string GetUserFromToken(string token);
    }
}
