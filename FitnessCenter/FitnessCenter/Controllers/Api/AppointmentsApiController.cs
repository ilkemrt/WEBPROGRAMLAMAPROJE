using FitnessCenter.Web.Data;
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

        // GET: api/appointments/member/USERID
        [HttpGet("member/{userId}")]
        public async Task<IActionResult> GetMemberAppointments(string userId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Trainer)
                .Where(a => a.MemberId == userId)
                .Select(a => new
                {
                    a.Id,
                    Service = a.Service.Name,
                    Trainer = a.Trainer.FirstName + " " + a.Trainer.LastName,
                    a.StartTime,
                    a.Status
                })
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            return Ok(appointments);
        }
    }
}
