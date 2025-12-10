using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUPROJE.Data;
using SPORSALONUPROJE.Models;

namespace SPORSALONUPROJE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MusaitlikController : Controller
    {
        private readonly UygulamaDbContext _context;

        public MusaitlikController(UygulamaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int antrenorId)
        {
            var antrenor = await _context.Antrenorler.FindAsync(antrenorId);
            if (antrenor == null) return NotFound();

            ViewBag.AntrenorAd = antrenor.AdSoyad;
            ViewBag.AntrenorId = antrenorId;

            var saatler = await _context.AntrenorMusaitlikler
                .Where(m => m.AntrenorId == antrenorId)
                .ToListAsync();

            return View(saatler);
        }

        public IActionResult Ekle(int antrenorId)
        {
            var model = new AntrenorMusaitlik { AntrenorId = antrenorId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(AntrenorMusaitlik model)
        {
            if (ModelState.IsValid)
            {
                _context.AntrenorMusaitlikler.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { antrenorId = model.AntrenorId });
            }
            return View(model);
        }

        public async Task<IActionResult> Sil(int id)
        {
            var musaitlik = await _context.AntrenorMusaitlikler.FindAsync(id);
            if (musaitlik == null) return NotFound();

            _context.AntrenorMusaitlikler.Remove(musaitlik);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { antrenorId = musaitlik.AntrenorId });
        }
    }
}
