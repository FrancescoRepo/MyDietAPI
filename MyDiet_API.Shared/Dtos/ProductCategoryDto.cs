using System.ComponentModel.DataAnnotations;

namespace MyDiet_API.Shared.Dtos
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }
    }
}
