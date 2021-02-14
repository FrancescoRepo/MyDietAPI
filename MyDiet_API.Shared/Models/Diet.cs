using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyDiet_API.Shared.Models
{
    public class Diet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual IList<DietMeal> DietMeal { get; set; }
    }
}
