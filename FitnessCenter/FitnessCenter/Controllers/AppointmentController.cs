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

        public AppointmentController(AppDbContext context,
                                     UserManager<ApplicationUser> userManager,
                                     AppointmentService appointmentService)
        {
            _context = context;
            _userManager = userManager;
            _appointmentService = appointmentService;
        }

        // -----------------------------
        // HİZMETTEN RANDEVU AL
        // -----------------------------
        public async Task<IActionResult> Create(int serviceId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");


            var service = await _context.Services.FindAsync(serviceId);
            var trainers = await _context.Trainers
                .Where(t => t.ServiceId == serviceId)
                .ToListAsync();

            var vm = new AppointmentCreateViewModel
            {
                ServiceId = serviceId,
                ServiceName = service.Name,
                Price = service.Price,
                Duration = service.Duration,
                Trainers = trainers
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentCreateViewModel vm)
        {
            var userId = _userManager.GetUserId(User);
            var service = await _context.Services.FindAsync(vm.ServiceId);

            // Müsaitlik kontrolü
            var available = await _appointmentService.IsTrainerAvailable(
                vm.TrainerId,
                vm.StartTime,
                service.Duration
            );

            if (!available)
            {
                ModelState.AddModelError("", "Bu saat dolu! Lütfen başka bir zaman seçin.");
                vm.Trainers = await _context.Trainers.Where(t => t.ServiceId == vm.ServiceId).ToListAsync();
                return View(vm);
            }

            var appointment = new Appointment
            {
                MemberId = userId,
                TrainerId = vm.TrainerId,
                ServiceId = vm.ServiceId,
                StartTime = vm.StartTime,
                Duration = service.Duration,
                Price = service.Price,
                Status = AppointmentStatus.Pending
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyAppointments");
        }

        // -----------------------------
        // ANTRENÖRDEN RANDEVU AL
        // -----------------------------
        public async Task<IActionResult> CreateByTrainer(int trainerId)
        {
            var trainer = await _context.Trainers
                .Include(t => t.Service)
                .FirstOrDefaultAsync(t => t.Id == trainerId);

            var vm = new AppointmentCreateViewModel
            {
                TrainerId = trainerId,
                TrainerName = $"{trainer.FirstName} {trainer.LastName}",
                ServiceId = trainer.ServiceId,
                ServiceName = trainer.Service.Name,
                Price = trainer.Service.Price,
                Duration = trainer.Service.Duration,
            };

            return View("Create", vm);
        }

        // -----------------------------
        // RANDEVULARIM
        // -----------------------------
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
