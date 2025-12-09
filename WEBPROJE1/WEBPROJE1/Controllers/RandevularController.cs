using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBPROJE1.Data;
using WEBPROJE1.Models;
using Microsoft.AspNetCore.Authorization;

namespace WEBPROJE1.Controllers
{
    [Authorize]
    public class RandevularController : Controller
    {
        private readonly SporContext _context;
        private readonly UserManager<Uye> _userManager;

        public RandevularController(SporContext context, UserManager<Uye> userManager)
        {
            _context = context;
            _userManager = userManager;
        }




        // GET: Randevular
        public async Task<IActionResult> Index()
        {
            // Giriş yapan kullanıcıyı bul
            var user = await _userManager.GetUserAsync(User);

            // Temel sorgu (Tabloları birleştirerek getir: Antrenor, Hizmet ve Uye bilgileri lazım)
            var randevularQuery = _context.Randevular
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Include(r => r.Uye)
                .AsQueryable();

            // EĞER ADMİN DEĞİLSE (Normal Üye ise) -> Sadece kendi randevularını görsün
            // Not: "Admin" rolü ismini DbInitializer'da nasıl yazdıysan birebir aynı olmalı.
            if (!User.IsInRole("Admin"))
            {
                randevularQuery = randevularQuery.Where(r => r.UyeId == user.Id);
            }

            // Tarihe göre yeniden eskiye sıralayalım
            return View(await randevularQuery.OrderByDescending(x => x.RandevuTarihi).ToListAsync());
        }

        // GET: Randevular/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevular
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Include(r => r.Uye)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // GET: Randevular/Create
        [Authorize(Roles = "Uye")]
        public IActionResult Create()
        {
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "Id", "Id");
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "Id", "Id");
            ViewData["UyeId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }
        // Randevu Onaylama (Sadece Admin yapabilir)
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Onayla(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return NotFound();

            randevu.Durum = RandevuDurumu.Onaylandi; // Durumu güncelle
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Randevu İptal (Hem Admin hem randevu sahibi yapabilir aslında ama şimdilik Admin diyelim)
        public async Task<IActionResult> IptalEt(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null) return NotFound();

            randevu.Durum = RandevuDurumu.IptalEdildi; // Durumu güncelle
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        // POST: Randevular/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Randevular/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Uye")]
        public async Task<IActionResult> Create([Bind("Id,RandevuTarihi,AntrenorId,HizmetId")] Randevu randevu)
        {
            // 1. KURAL: Geçmişe randevu alınamaz
            if (randevu.RandevuTarihi < DateTime.Now)
            {
                ModelState.AddModelError("", "Geçmiş bir tarihe randevu alamazsınız.");
            }

            // 2. KURAL: Antrenör o saatte dolu mu?
            // Veritabanına soruyoruz: Bu antrenörün, bu tarihte, iptal edilmemiş başka bir randevusu var mı?
            bool antrenorDoluMu = _context.Randevular.Any(x =>
                x.AntrenorId == randevu.AntrenorId &&
                x.RandevuTarihi == randevu.RandevuTarihi &&
                x.Durum != RandevuDurumu.IptalEdildi);

            if (antrenorDoluMu)
            {
                ModelState.AddModelError("", "Seçtiğiniz antrenör bu saatte maalesef dolu.");
            }

            // Eğer hata yoksa ve model geçerliyse kaydet
            if (ModelState.IsValid)
            {
                // Otomatik atamalar
                randevu.OlusturulmaTarihi = DateTime.Now;
                randevu.Durum = RandevuDurumu.Beklemede;

                // Hangi kullanıcı randevu alıyor? (Giriş yapan kullanıcıyı bul)
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    randevu.UyeId = user.Id; // Giriş yapan kullanıcının ID'sini ekle
                }
                else
                {
                    // Eğer giriş yapmamışsa hata ver veya Admin panelinden ekleniyorsa boş bırakma
                    // Şimdilik Admin ekliyor varsayalım, burayı geçelim.
                    // Ama normalde: return RedirectToAction("Login", "Account");
                }

                _context.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa formu tekrar doldurup göster (Dropdownları yenilememiz lazım)
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "Id", "TamAd", randevu.AntrenorId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "Id", "Ad", randevu.HizmetId);

            return View(randevu);
        }

        // GET: Randevular/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "Id", "Id", randevu.AntrenorId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "Id", "Id", randevu.HizmetId);
            ViewData["UyeId"] = new SelectList(_context.Users, "Id", "Id", randevu.UyeId);
            return View(randevu);
        }

        // POST: Randevular/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RandevuTarihi,OlusturulmaTarihi,Durum,UyeId,AntrenorId,HizmetId")] Randevu randevu)
        {
            if (id != randevu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuExists(randevu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "Id", "Id", randevu.AntrenorId);
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "Id", "Id", randevu.HizmetId);
            ViewData["UyeId"] = new SelectList(_context.Users, "Id", "Id", randevu.UyeId);
            return View(randevu);
        }

        // GET: Randevular/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var randevu = await _context.Randevular
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Include(r => r.Uye)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // POST: Randevular/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var randevu = await _context.Randevular.FindAsync(id);
            if (randevu != null)
            {
                _context.Randevular.Remove(randevu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RandevuExists(int id)
        {
            return _context.Randevular.Any(e => e.Id == id);
        }
    }
}
