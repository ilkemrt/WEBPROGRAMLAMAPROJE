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
            // 1) ACTION'A GİRİYOR MU?
            Console.WriteLine(">>> [ServiceController] CREATE POST ÇALIŞTI");
            Console.WriteLine($"    Model null mu? => {(model == null ? "EVET" : "HAYIR")}");
            Console.WriteLine($"    Name: {model?.Name}");
            Console.WriteLine($"    Duration: {model?.Duration}");
            Console.WriteLine($"    Price: {model?.Price}");

            // 2) MODELSTATE HATALARINI YAZDIR
            if (!ModelState.IsValid)
            {
                Console.WriteLine(">>> MODELSTATE HATALI, DETAYLAR:");
                foreach (var kv in ModelState)
                {
                    var key = kv.Key;
                    foreach (var err in kv.Value.Errors)
                    {
                        Console.WriteLine($"    [{key}] => {err.ErrorMessage}");
                    }
                }

                // Hataları ekranda da görelim
                return View(model);
            }

            // 3) AYNI İSİMDE HİZMET VAR MI?
            var existing = await _context.Services
                .FirstOrDefaultAsync(x => x.Name.ToLower() == model.Name.ToLower());

            if (existing != null)
            {
                Console.WriteLine(">>> AYNI İSİMDE HİZMET VAR, EKLENMEDİ.");
                ModelState.AddModelError("Name", "Bu hizmet adı zaten mevcut!");
                return View(model);
            }

            // 4) DB'YE KAYDET
            _context.Services.Add(model);
            var affected = await _context.SaveChangesAsync();

            Console.WriteLine($">>> SAVECHANGES ÇALIŞTI. Etkilenen satır: {affected}");

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
