using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Web.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(500)]
        public string Biography { get; set; }

        public string ImageUrl { get; set; }

        // Her antrenör 1 hizmet verir
        [Required]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        // Navigation
        public ICollection<Appointment> Appointments { get; set; }
    }
}
