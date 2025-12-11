using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrainerController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public TrainerController(AppDbContext context)
        {
            _context = context;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trainers
                .Include(t => t.Service)
                .ToListAsync();

            return View(trainers);
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // CREATE POST
        [HttpPost]
        public async Task<IActionResult> Create(Trainer model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
                return View(model);
            }

            _context.Trainers.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            ViewBag.Services = new SelectList(_context.Services, "Id", "Name", trainer.ServiceId);
            return View(trainer);
        }

        // EDIT POST
        [HttpPost]
        public async Task<IActionResult> Edit(Trainer model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = new SelectList(_context.Services, "Id", "Name", model.ServiceId);
                return View(model);
            }

            _context.Trainers.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
