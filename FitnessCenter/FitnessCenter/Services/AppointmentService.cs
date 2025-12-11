using FitnessCenter.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Services
{
    public class AppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        // Antrenör belirli bir saatte müsait mi?
        public async Task<bool> IsTrainerAvailable(int trainerId, DateTime startTime, int duration)
        {
            var endTime = startTime.AddMinutes(duration);

            return !await _context.Appointments.AnyAsync(a =>
                a.TrainerId == trainerId &&
                a.Status != Models.AppointmentStatus.Rejected && // Reddedilenler çakışma sayılmaz
                (
                    (startTime >= a.StartTime && startTime < a.StartTime.AddMinutes(a.Duration)) ||
                    (endTime > a.StartTime && endTime <= a.StartTime.AddMinutes(a.Duration))
                )
            );
        }
    }
}
