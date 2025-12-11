using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Areas.Admin.Controllers
{
    public class MemberController : AdminBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Üyeleri listele
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
    }
}
