using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiet_API.Shared.Models
{
    public class DietMeal
    {
        public int DietId { get; set; }
        public Diet Diet { get; set; }

        public int MealId { get; set; }
        public Meal Meal { get; set; }
    }
}
