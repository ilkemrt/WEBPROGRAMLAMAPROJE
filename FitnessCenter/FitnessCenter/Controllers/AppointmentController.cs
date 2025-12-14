using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
using FitnessCenter.Web.Models.ViewModels;
using FitnessCenter.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Controllers
{
    [Authorize(Roles = "Member")]
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppointmentService _appointmentService;

        public AppointmentController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            AppointmentService appointmentService)
        {
            _context = context;
            _userManager = userManager;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int serviceId)
        {


            var service = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == serviceId);
            if (service == null) return NotFound();

            var trainers = await _context.Trainers
                .AsNoTracking()
                .Where(t => t.ServiceId == serviceId)
                .Select(t => new TrainerSelectItem
                {
                    Id = t.Id,
                    FullName = t.FirstName + " " + t.LastName
                })
                .ToListAsync();

            if (trainers.Count == 0)
            {
                // Hizmet var ama eğitmen yoksa kullanıcıya düzgün mesaj verelim
                TempData["Error"] = "Bu hizmet için henüz antrenör tanımlanmamış.";
                return RedirectToAction("Details", "Service", new { id = serviceId });
            }

            var vm = new AppointmentCreateViewModel
            {
                ServiceId = service.Id,
                ServiceName = service.Name,
                Duration = service.Duration,
                Price = service.Price,
                AvailableTrainers = trainers,
                Date = DateTime.Today,
                TrainerId = trainers[0].Id // default ilk trainer
            };

            vm.AvailableHours = await _appointmentService.GetAvailableHours(vm.TrainerId, vm.Date, vm.Duration);

            // default saat: ilk müsait
            if (vm.AvailableHours.Count > 0)
                vm.StartHour = vm.AvailableHours[0];

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateViewModel vm)
        {

            var service = await _context.Services.FindAsync(vm.ServiceId);
            if (service == null) return NotFound();

            // dropdownlar her durumda dolmalı
            vm.AvailableTrainers = await _context.Trainers
                .AsNoTracking()
                .Where(t => t.ServiceId == vm.ServiceId)
                .Select(t => new TrainerSelectItem
                {
                    Id = t.Id,
                    FullName = t.FirstName + " " + t.LastName
                })
                .ToListAsync();

            vm.Duration = service.Duration;
            vm.Price = service.Price;
            vm.ServiceName = service.Name;

            // Trainer seçildiyse saatleri tekrar doldur
            if (vm.TrainerId > 0)
                vm.AvailableHours = await _appointmentService.GetAvailableHours(vm.TrainerId, vm.Date, vm.Duration);

            if (!ModelState.IsValid)
                return View(vm);

            // Güvenlik: Trainer gerçekten bu service'e ait mi?
            var trainerOk = await _context.Trainers.AnyAsync(t => t.Id == vm.TrainerId && t.ServiceId == vm.ServiceId);
            if (!trainerOk)
            {
                ModelState.AddModelError(nameof(vm.TrainerId), "Seçilen antrenör bu hizmete ait değil.");
                return View(vm);
            }

            var startTime = vm.Date.Date + vm.StartHour;

            var isAvailable = await _appointmentService.IsTrainerAvailable(vm.TrainerId, startTime, service.Duration);
            if (!isAvailable)
            {
                ModelState.AddModelError("", "Seçilen gün/saat için antrenör müsait değil.");
                return View(vm);
            }

            var appointment = new Appointment
            {
                MemberId = _userManager.GetUserId(User)!,
                TrainerId = vm.TrainerId,
                ServiceId = vm.ServiceId,
                StartTime = startTime,
                Duration = service.Duration,
                Price = service.Price,
                Status = AppointmentStatus.Pending
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyAppointments));
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableHours(int trainerId, int serviceId, DateTime date)
        {
            // 1. Hizmetin gerçek süresini bul
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null) return BadRequest();

            // 2. O süreyi metoda gönder (120 yerine service.Duration)
            var hours = await _appointmentService.GetAvailableHours(trainerId, date, service.Duration);

            return Json(hours.Select(h => h.ToString(@"hh\:mm")));
        }



        public async Task<IActionResult> MyAppointments()
        {
            var userId = _userManager.GetUserId(User);

            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .Where(a => a.MemberId == userId)
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            return View(appointments);
        }
    }
}

