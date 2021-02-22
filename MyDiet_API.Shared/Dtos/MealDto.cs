using MyDiet_API.Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyDiet_API.Shared.Dtos
{
    public class MealDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public IList<DietMeal> DietMeal { get; set; }

        public IList<MealProduct> MealProduct { get; set; }

        public override string ToString()
        {
            return $"[Id: {Id}," +
                $"Name: {Name}," +
                $"Description: {Description}]";
        }
    }
}
