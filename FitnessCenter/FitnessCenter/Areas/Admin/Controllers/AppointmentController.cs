using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .ToListAsync();

            return View(appointments);
        }

        // APPROVE
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.Status = AppointmentStatus.Approved;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // REJECT
        public async Task<IActionResult> Reject(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.Status = AppointmentStatus.Rejected;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
