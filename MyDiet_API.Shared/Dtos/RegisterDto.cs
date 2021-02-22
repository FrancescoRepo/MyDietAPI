using System.ComponentModel.DataAnnotations;

namespace MyDiet_API.Shared.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string ConfirmPassword { get; set; }

        public override string ToString()
        {
            return $"[Email: {Email}]";
        }
    }
}
