using FitnessCenter.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        // Hizmet Listesi
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        // Hizmet Detayı
        public async Task<IActionResult> Details(int id)
        {
            var service = await _context.Services
            .Include(s => s.Trainers)
        .FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
                return NotFound();

            return View(service);
        }
    }
}
