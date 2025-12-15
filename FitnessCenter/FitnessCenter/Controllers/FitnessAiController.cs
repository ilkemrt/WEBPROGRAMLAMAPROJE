using FitnessCenter.Web.Models.Ai;
using FitnessCenter.Web.Models.ViewModels;
using FitnessCenter.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenter.Web.Controllers
{
    public class FitnessAiController : Controller
    {
        private readonly GoogleAiService _ai;

        public FitnessAiController(GoogleAiService ai)
        {
            _ai = ai;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AiTrainerRequestVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GeneratePlan(AiTrainerRequestVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View("Index", vm);

            try
            {
                var prompt = PromptBuilder.Build(vm);
                AiWorkoutPlan plan = await _ai.GeneratePlanAsync(prompt, ct);
                return View("Result", plan);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index", vm);
            }
        }
    }
}
