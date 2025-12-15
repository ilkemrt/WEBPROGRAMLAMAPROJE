namespace FitnessCenter.Web.Models.Ai
{
    public class AiWorkoutPlan
    {
        public string Title { get; set; } = "";
        public string Notes { get; set; } = "";
        public List<AiDayPlan> WeeklyPlan { get; set; } = new();
        public AiNutritionPlan Nutrition { get; set; } = new();
    }

    public class AiDayPlan
    {
        public string Day { get; set; } = "";
        public string Focus { get; set; } = "";
        public List<AiExercise> Exercises { get; set; } = new();
        public string Cardio { get; set; } = "";
    }

    public class AiExercise
    {
        public string Name { get; set; } = "";
        public int Sets { get; set; }
        public string Reps { get; set; } = "";
        public string RestSec { get; set; } = "";
    }

    public class AiNutritionPlan
    {
        public int DailyCalories { get; set; }
        public int ProteinGr { get; set; }
        public int CarbsGr { get; set; }
        public int FatGr { get; set; }
        public List<string> MealIdeas { get; set; } = new();
    }
}
