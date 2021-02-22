using System.ComponentModel.DataAnnotations;

namespace MyDiet_API.Shared.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]

        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public LoginDto()
        {
        }

        public LoginDto(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public override string ToString()
        {
            return $"[Email: {Email}]";
        }
    }
}
