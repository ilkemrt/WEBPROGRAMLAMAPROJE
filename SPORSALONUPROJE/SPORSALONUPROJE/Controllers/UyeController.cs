using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPORSALONUPROJE.Controllers
{
    [Authorize(Roles = "Üye")]
    public class UyeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
