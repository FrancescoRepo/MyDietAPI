using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiet_API.Shared.Models
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual IList<DietMeal> DietMeal { get; set; }

        public virtual IList<MealProduct> MealProduct { get; set; }
    }
}
