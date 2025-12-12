using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Web.Models
{
    public class TrainerWorkingHour
    {
        public int Id { get; set; }

        [Required]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }


    }
}
