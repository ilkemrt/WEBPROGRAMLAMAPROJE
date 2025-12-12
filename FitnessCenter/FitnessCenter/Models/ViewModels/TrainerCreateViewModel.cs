using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Web.Models.ViewModels
{
    public class TrainerCreateViewModel
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        public string? Biography { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public List<DayOfWeek> WorkingDays { get; set; } = new();

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }
    }
}
