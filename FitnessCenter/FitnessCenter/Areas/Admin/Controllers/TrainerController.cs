using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
using FitnessCenter.Web.Models.ViewModels;
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
                .Include(t => t.WorkingHours)
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
        public async Task<IActionResult> Create(TrainerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = new SelectList(_context.Services, "Id", "Name");
                return View(model);
            }

            var trainer = new Trainer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Biography = model.Biography,
                ImageUrl = model.ImageUrl,
                ServiceId = model.ServiceId
            };

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();

            // 🔥 ÇALIŞMA GÜNLERİ KAYDI
            foreach (var day in model.WorkingDays)
            {
                var workingHour = new TrainerWorkingHour
                {
                    TrainerId = trainer.Id,
                    DayOfWeek = day,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime
                };

                _context.TrainerWorkingHours.Add(workingHour);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _context.Trainers
                .Include(t => t.WorkingHours)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
                return NotFound();

            var vm = new TrainerEditViewModel
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Biography = trainer.Biography,
                ImageUrl = trainer.ImageUrl,
                ServiceId = trainer.ServiceId,
                WorkingDays = trainer.WorkingHours.Select(w => w.DayOfWeek).ToList(),
                StartTime = trainer.WorkingHours.FirstOrDefault()?.StartTime ?? new TimeOnly(8, 0),
                EndTime = trainer.WorkingHours.FirstOrDefault()?.EndTime ?? new TimeOnly(17, 0)
            };

            ViewBag.Services = new SelectList(_context.Services, "Id", "Name", vm.ServiceId);
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(TrainerEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = new SelectList(_context.Services, "Id", "Name", vm.ServiceId);
                return View(vm);
            }

            var trainer = await _context.Trainers
                .Include(t => t.WorkingHours)
                .FirstOrDefaultAsync(t => t.Id == vm.Id);

            if (trainer == null)
                return NotFound();

            trainer.FirstName = vm.FirstName;
            trainer.LastName = vm.LastName;
            trainer.Biography = vm.Biography;
            trainer.ImageUrl = vm.ImageUrl;
            trainer.ServiceId = vm.ServiceId;

            // 🔥 eski saatleri sil
            _context.TrainerWorkingHours.RemoveRange(trainer.WorkingHours);

            // 🔥 yenilerini ekle
            foreach (var day in vm.WorkingDays)
            {
                _context.TrainerWorkingHours.Add(new TrainerWorkingHour
                {
                    TrainerId = trainer.Id,
                    DayOfWeek = day,
                    StartTime = vm.StartTime,
                    EndTime = vm.EndTime
                });
            }

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
