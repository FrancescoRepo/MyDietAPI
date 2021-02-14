using MyDiet_API.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyDiet_API.Shared.Dtos
{
    public class DietDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "patient")]
        public PatientDto PatientDto { get; set; }

        public IList<DietMeal> DietMeal { get; set; }
    }
}
