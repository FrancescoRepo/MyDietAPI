using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDiet_API.Shared.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FiscalCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Phone { get; set; }

        [ForeignKey("DietId")]
        public Diet Diet { get; set; }

        public int? DietId { get; set; }

        [NotMapped]
        public decimal Weight { get; set; }
    }
}
