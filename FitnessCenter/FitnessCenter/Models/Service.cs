using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Web.Models
{
    public class Service
    {
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
        public ICollection<Trainer> Trainers { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
