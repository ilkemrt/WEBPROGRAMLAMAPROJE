using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEBPROJE1.Models;

namespace WEBPROJE1.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Uye> _signInManager;
        private readonly UserManager<Uye> _userManager;

        public AccountController(SignInManager<Uye> signInManager, UserManager<Uye> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Giriş yapmayı dene (KullanıcıAdı/Email, Şifre, Beni Hatırla, Kilitleme)
                // Not: Identity varsayılan olarak UserName bekler, biz Email kullanıyorsak UserName yerine Email vereceğiz.
                // Eğer sistemin UserName ile çalışıyorsa buraya model.Email yerine o kullanıcının username'ini bulup vermelisin.
                // Genelde DbInitializer'da UserName = Email yapmıştık, o yüzden sorun çıkmaz.

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // Giriş başarılıysa Ana Sayfaya veya Admin paneline git
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
            }
            return View(model);
        }

        // Çıkış Yapma (Logout)
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Yeni kullanıcı nesnesi oluşturuyoruz
                var user = new Uye
                {
                    UserName = model.Email, // Kullanıcı adı email olsun
                    Email = model.Email,
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    DogumTarihi = DateTime.Now // Şimdilik rastgele atadık, formdan da alabilirsin
                };

                // 1. Kullanıcıyı Identity tablosuna kaydet
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 2. Kullanıcıya otomatik olarak "Uye" rolünü ver
                    await _userManager.AddToRoleAsync(user, "Uye");

                    // 3. Kayıt bitti, otomatik giriş yaptır ve ana sayfaya at
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Hata varsa (Örn: Şifre çok basit) ekrana bas
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }




    }
}