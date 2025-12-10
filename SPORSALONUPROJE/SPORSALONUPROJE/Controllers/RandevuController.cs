using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPORSALONUPROJE.Data;
using SPORSALONUPROJE.Models;
using SPORSALONUPROJE.ViewModels;

namespace SPORSALONUPROJE.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly UserManager<Uye> _userManager;

        public RandevuController(UygulamaDbContext context, UserManager<Uye> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Uye")]
        public async Task<IActionResult> RandevuAl()
        {
            var model = new RandevuFormViewModel
            {
                Antrenorler = await _context.Antrenorler.ToListAsync(),
                Hizmetler = await _context.Hizmetler.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Uye")]
        public async Task<IActionResult> RandevuAl(RandevuFormViewModel model)
        {
            var uye = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                model.Antrenorler = await _context.Antrenorler.ToListAsync();
                model.Hizmetler = await _context.Hizmetler.ToListAsync();
                return View(model);
            }

            // 1. Antrenör o saatte müsait mi?
            var gun = model.BaslangicZamani.DayOfWeek;
            var saat = model.BaslangicZamani.TimeOfDay;

            var musaitMi = await _context.AntrenorMusaitlikler.AnyAsync(m =>
                m.AntrenorId == model.AntrenorId &&
                m.Gun == gun &&
                saat >= m.BaslangicSaat &&
                saat <= m.BitisSaat);

            if (!musaitMi)
            {
                ModelState.AddModelError("", "Antrenör bu saatte müsait değil.");
                model.Antrenorler = await _context.Antrenorler.ToListAsync();
                model.Hizmetler = await _context.Hizmetler.ToListAsync();
                return View(model);
            }

            // 2. Aynı saatte randevusu var mı?
            bool cakisma = await _context.Randevular.AnyAsync(r =>
                r.UyeId == uye.Id &&
                r.BaslangicZamani == model.BaslangicZamani &&
                !r.IptalEdildiMi);

            if (cakisma)
            {
                ModelState.AddModelError("", "Bu saatte zaten randevunuz var.");
                model.Antrenorler = await _context.Antrenorler.ToListAsync();
                model.Hizmetler = await _context.Hizmetler.ToListAsync();
                return View(model);
            }

            // Kaydet
            var randevu = new Randevu
            {
                AntrenorId = model.AntrenorId,
                HizmetId = model.HizmetId,
                BaslangicZamani = model.BaslangicZamani,
                UyeId = uye.Id,
                OnaylandiMi = false,
                IptalEdildiMi = false
            };

            _context.Randevular.Add(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction("Randevularim");
        }

        [Authorize(Roles = "Uye")]
        public async Task<IActionResult> Randevularim()
        {
            var uye = await _userManager.GetUserAsync(User);

            var randevular = await _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Antrenor)
                .Where(r => r.UyeId == uye.Id && !r.IptalEdildiMi && r.OnaylandiMi)
                .ToListAsync();

            return View(randevular);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TumRandevular()
        {
            var randevular = await _context.Randevular
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Include(r => r.Uye)
                .ToListAsync();

            return View(randevular);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IptalEt(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return NotFound();

            randevu.IptalEdildiMi = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("TumRandevular");
        }

    }
}
