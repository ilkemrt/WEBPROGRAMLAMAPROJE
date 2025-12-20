using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Web.Models.ViewModels
{
    public class TrainerEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Biography { get; set; }
        public string ImageUrl { get; set; }

        [Required]
        public int ServiceId { get; set; }

        //  çalışma düzeni
        public List<DayOfWeek> WorkingDays { get; set; } = new();

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

}
