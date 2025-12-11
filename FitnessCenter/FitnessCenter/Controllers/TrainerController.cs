using FitnessCenter.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Controllers
{
    public class TrainerController : Controller
    {
        private readonly AppDbContext _context;

        public TrainerController(AppDbContext context)
        {
            _context = context;
        }

        // TÜM ANTRENÖRLERİ LİSTELE
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers
                .Include(t => t.Service)
                .ToListAsync();

            return View(trainers);
        }

        // DETAY SAYFASI (İSTEĞE BAĞLI)
        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _context.Trainers
                .Include(t => t.Service)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
                return NotFound();

            return View(trainer);
        }
    }
}
