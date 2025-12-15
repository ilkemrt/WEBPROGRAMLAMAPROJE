using System.ComponentModel.DataAnnotations;

namespace FitnessCenter.Web.Models.ViewModels
{
    public class AiTrainerRequestVm
    {
        [Range(12, 90)]
        public int Age { get; set; }

        [Range(120, 230)]
        public int HeightCm { get; set; }

        [Range(30, 250)]
        public int WeightKg { get; set; }

        [Range(0, 100)]
        public int BodyFatPercent { get; set; }

        [Required]
        public string Goal { get; set; } = "LoseFat"; // LoseFat | GainMuscle | Fit

        [Range(2, 6)]
        public int WorkoutDaysPerWeek { get; set; } = 3;

        [Required]
        public string Experience { get; set; } = "Beginner"; // Beginner | Intermediate | Advanced
    }
}
