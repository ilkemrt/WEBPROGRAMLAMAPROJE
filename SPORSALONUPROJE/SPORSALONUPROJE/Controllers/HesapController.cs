using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SPORSALONUPROJE.Models;
using SPORSALONUPROJE.ViewModels;

namespace SPORSALONUPROJE.Controllers
{
    public class HesapController : Controller
    {
        private readonly UserManager<Uye> _userManager;
        private readonly SignInManager<Uye> _signInManager;

        public HesapController(UserManager<Uye> userManager, SignInManager<Uye> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Hesap/Giris
        public IActionResult Giris()
        {
            return View();
        }

        // POST: /Hesap/Giris
        [HttpPost]
        public async Task<IActionResult> Giris(GirisViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var sonuc = await _signInManager.PasswordSignInAsync(model.Eposta, model.Sifre, model.BeniHatirla, false);

            if (sonuc.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Geçersiz giriş bilgileri.");
            return View(model);
        }

        // GET: /Hesap/Kayit
        public IActionResult Kayit()
        {
            return View();
        }

        // POST: /Hesap/Kayit
        [HttpPost]
        public async Task<IActionResult> Kayit(KayitViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var uye = new Uye
            {
                UserName = model.Eposta,
                Email = model.Eposta,
                AdSoyad = model.AdSoyad,
                Rol = "Üye"
            };

            var sonuc = await _userManager.CreateAsync(uye, model.Sifre);

            if (sonuc.Succeeded)
            {
                await _userManager.AddToRoleAsync(uye, "Uye");
                await _signInManager.SignInAsync(uye, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var hata in sonuc.Errors)
            {
                ModelState.AddModelError("", hata.Description);
            }

            return View(model);
        }

        // GET: /Hesap/Cikis
        public async Task<IActionResult> Cikis()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Giris", "Hesap");
        }
    }
}
