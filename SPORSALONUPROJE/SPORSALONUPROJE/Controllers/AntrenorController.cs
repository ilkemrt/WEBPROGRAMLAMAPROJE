using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUPROJE.Data;
using SPORSALONUPROJE.Models;
using SPORSALONUPROJE.ViewModels;

namespace SPORSALONUPROJE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AntrenorController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AntrenorController(UygulamaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var antrenorler = await _context.Antrenorler
                .Include(a => a.AntrenorHizmetleri)
                    .ThenInclude(ah => ah.Hizmet)
                .ToListAsync();

            return View(antrenorler);
        }

        public async Task<IActionResult> Ekle()
        {
            var hizmetler = await _context.Hizmetler.ToListAsync();
            var model = new AntrenorFormViewModel
            {
                MevcutHizmetler = hizmetler
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(AntrenorFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.MevcutHizmetler = await _context.Hizmetler.ToListAsync();
                return View(model);
            }

            var antrenor = new Antrenor
            {
                AdSoyad = model.AdSoyad,
                Aciklama = model.Aciklama,
                AntrenorHizmetleri = model.SecilenHizmetler?.Select(hid => new AntrenorHizmet
                {
                    HizmetId = hid
                }).ToList()
            };

            _context.Antrenorler.Add(antrenor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Guncelle(int id)
        {
            var antrenor = await _context.Antrenorler
                .Include(a => a.AntrenorHizmetleri)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (antrenor == null) return NotFound();

            var model = new AntrenorFormViewModel
            {
                Id = antrenor.Id,
                AdSoyad = antrenor.AdSoyad,
                Aciklama = antrenor.Aciklama,
                SecilenHizmetler = antrenor.AntrenorHizmetleri.Select(ah => ah.HizmetId).ToList(),
                MevcutHizmetler = await _context.Hizmetler.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(AntrenorFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.MevcutHizmetler = await _context.Hizmetler.ToListAsync();
                return View(model);
            }

            var antrenor = await _context.Antrenorler
                .Include(a => a.AntrenorHizmetleri)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (antrenor == null) return NotFound();

            antrenor.AdSoyad = model.AdSoyad;
            antrenor.Aciklama = model.Aciklama;

            // Hizmet ilişkilerini güncelle
            antrenor.AntrenorHizmetleri.Clear();
            if (model.SecilenHizmetler != null)
            {
                antrenor.AntrenorHizmetleri = model.SecilenHizmetler.Select(hid => new AntrenorHizmet
                {
                    HizmetId = hid
                }).ToList();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Sil(int id)
        {
            var antrenor = await _context.Antrenorler.FindAsync(id);
            if (antrenor == null) return NotFound();

            _context.Antrenorler.Remove(antrenor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
