using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Web.Models.ViewModels
{
    public class AppointmentCreateViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = "";
      
        public int TrainerId { get; set; }

        // sadece tarih
        public DateTime Date { get; set; } = DateTime.Today;

        // sadece saat (08:00 vb)
        [Required(ErrorMessage = "Saat seçmek zorunludur")]
        public TimeSpan? StartHour { get; set; }

        public int Duration { get; set; }
        public decimal Price { get; set; }

        public List<TrainerSelectItem> AvailableTrainers { get; set; } = new();
        public List<TimeSpan> AvailableHours { get; set; } = new();
    }
}
