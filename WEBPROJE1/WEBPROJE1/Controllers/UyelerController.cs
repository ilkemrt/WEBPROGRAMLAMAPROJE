using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBPROJE1.Models;

namespace ProjeIsmi.Controllers
{
    // BU SAYFAYA SADECE ADMİNLER GİREBİLİR!
    [Authorize(Roles = "Admin")]
    public class UyelerController : Controller
    {
        private readonly UserManager<Uye> _userManager;

        public UyelerController(UserManager<Uye> userManager)
        {
            _userManager = userManager;
        }

        // Üyeleri Listele
        public async Task<IActionResult> Index()
        {
            // Veritabanındaki tüm üyeleri çek
            var uyeler = await _userManager.Users.ToListAsync();
            return View(uyeler);
        }

        // Üye Silme İşlemi
        [HttpPost]
        public async Task<IActionResult> Sil(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}