using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Controllers.Api
{
    [ApiController]
    [Route("api/AppointmentsApi")]
    public class AppointmentsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentsApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AppointmentsApi
        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .Select(a => new
                {
                    a.Id,
                    a.MemberId,
                    Service = a.Service.Name,
                    Trainer = a.Trainer.FirstName + " " + a.Trainer.LastName,
                    a.StartTime,
                    Status = a.Status == AppointmentStatus.Pending
                        ? "Beklemede"
                        : a.Status == AppointmentStatus.Approved
                            ? "Onaylandı"
                            : "Reddedildi",
                    a.Price
                })
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            return Ok(appointments);
        }
    }
}
