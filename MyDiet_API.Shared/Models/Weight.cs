using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDiet_API.Shared.Models
{
    public class Weight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal WeightValue { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
