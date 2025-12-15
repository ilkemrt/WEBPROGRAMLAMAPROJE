using FitnessCenter.Web.Models.ViewModels;

namespace FitnessCenter.Web.Services
{
    public static class PromptBuilder
    {
        public static string Build(AiTrainerRequestVm vm)
        {
            return $@"
You are a certified strength coach and nutritionist.

Return STRICT JSON only. No markdown. No extra text.

User:
- Age: {vm.Age}
- HeightCm: {vm.HeightCm}
- WeightKg: {vm.WeightKg}
- BodyFatPercent: {vm.BodyFatPercent}
- Goal: {vm.Goal} (LoseFat | GainMuscle | Fit)
- WorkoutDaysPerWeek: {vm.WorkoutDaysPerWeek}
- Experience: {vm.Experience} (Beginner | Intermediate | Advanced)

Rules:
- Create a weekly plan with exactly {vm.WorkoutDaysPerWeek} training days and remaining rest days.
- Each workout day: 5-7 exercises, include sets/reps and rest.
- Keep exercises safe and realistic.
- Provide nutrition macros and 4-6 meal ideas.
- Use Turkish day names (Pazartesi, Salı, ...)
- Respond ONLY in TURKISH.
- DO NOT use English words.
- DO NOT mix languages.
- All exercise names MUST be Turkish.
- All explanations MUST be Turkish.

JSON format exactly:
{{
  ""title"": ""..."",
  ""notes"": ""..."",
  ""weeklyPlan"": [
    {{
      ""day"": ""Pazartesi"",
      ""focus"": ""..."",
      ""exercises"": [
        {{ ""name"": ""..."", ""sets"": 4, ""reps"": ""8-10"", ""restSec"": ""90"" }}
      ],
      ""cardio"": ""...""
    }}
  ],
  ""nutrition"": {{
    ""dailyCalories"": 0,
    ""proteinGr"": 0,
    ""carbsGr"": 0,
    ""fatGr"": 0,
    ""mealIdeas"": [""..."",""...""] 
  }}
}}
";
        }
    }
}
