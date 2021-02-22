using MyDiet_API.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyDiet_API.Shared.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public ProductCategoryDto ProductCategory { get; set; }

        public override string ToString()
        {
            return $"[Id: {Id}, Name: {Name}, Description: {Description}, ProductCategory: {ProductCategory.Description}]";
        }
    }
}
