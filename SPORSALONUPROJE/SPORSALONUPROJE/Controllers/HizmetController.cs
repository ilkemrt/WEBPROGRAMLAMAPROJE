using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUPROJE.Data;
using SPORSALONUPROJE.Models;

namespace SPORSALONUPROJE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HizmetController : Controller
    {
        private readonly UygulamaDbContext _context;

        public HizmetController(UygulamaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hizmetler = await _context.Hizmetler.ToListAsync();
            return View(hizmetler);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Hizmet model)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Guncelle(int id)
        {
            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet == null) return NotFound();
            return View(hizmet);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(Hizmet model)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Sil(int id)
        {
            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet == null) return NotFound();

            _context.Hizmetler.Remove(hizmet);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
