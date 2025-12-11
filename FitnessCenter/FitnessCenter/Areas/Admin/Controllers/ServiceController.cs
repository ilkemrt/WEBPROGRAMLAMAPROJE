using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        // Listeleme
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        // Create GET
        public IActionResult Create()
        {
            return View();
        }

        // Create POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service model)
        {

            // Aynı isimde hizmet var mı?
            var existing = await _context.Services
                .FirstOrDefaultAsync(x => x.Name.ToLower() == model.Name.ToLower());

            if (existing != null)
            {
                ModelState.AddModelError("Name", "Bu hizmet adı zaten mevcut!");
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            _context.Services.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Edit GET
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            return View(service);
        }

        // Edit POST
        [HttpPost]
        public async Task<IActionResult> Edit(Service model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Services.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Delete
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
