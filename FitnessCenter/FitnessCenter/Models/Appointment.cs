using FitnessCenter.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Web.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        // Üye
        [Required]
        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        // Antrenör
        [Required]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        // Hizmet
        [Required]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        // Randevu Bilgileri
        [Required]
        public DateTime StartTime { get; set; }

        [Range(1, 300)]
        public int Duration { get; set; }
        [Required]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        public AppointmentStatus Status { get; set; }
    }
}
