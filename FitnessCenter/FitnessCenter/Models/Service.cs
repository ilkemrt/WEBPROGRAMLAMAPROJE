using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;


namespace FitnessCenter.Web.Models
{
    public class Service
    {

        public Service()
        {
            Trainers = new List<Trainer>();
            Appointments = new List<Appointment>();
           
        }   

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [Range(1, 300)]
        public int Duration { get; set; }  // Dakika

        [Range(0, 10000)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        // Navigation

        [ValidateNever]
        public ICollection<Trainer> Trainers { get; set; }

        [ValidateNever]
        public ICollection<Appointment> Appointments { get; set; }


    }
}
