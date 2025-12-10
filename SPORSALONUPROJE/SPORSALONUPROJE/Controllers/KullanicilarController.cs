using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SPORSALONUPROJE.Models;

namespace SPORSALONUPROJE.Controllers
{


    [Authorize(Roles = "Admin")]
    public class KullanicilarController : Controller
    {

        public async Task<IActionResult> Sil(string id)
        {
            var uye = await _userManager.FindByIdAsync(id);
            if (uye == null)
            {
                return NotFound();
            }

            var roller = await _userManager.GetRolesAsync(uye);
            if (roller.Contains("Admin"))
            {
                TempData["Hata"] = "Admin kullanıcı silinemez.";
                return RedirectToAction("Index");
            }

            var result = await _userManager.DeleteAsync(uye);
            if (result.Succeeded)
            {
                TempData["Mesaj"] = "Kullanıcı başarıyla silindi.";
            }

            return RedirectToAction("Index");
        }

        private readonly UserManager<Uye> _userManager;

        public KullanicilarController(UserManager<Uye> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var uyeler = _userManager.Users.ToList();
            return View(uyeler);
        }
    }
}
