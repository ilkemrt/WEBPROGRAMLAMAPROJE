using FitnessCenter.Web.Data;
using FitnessCenter.Web.Models;
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

        // Trainer'ın seçilen gün çalışma aralığı var mı?
        public async Task<TrainerWorkingHour?> GetWorkingHour(int trainerId, DateTime date)
        {
            var day = date.DayOfWeek;

            return await _context.TrainerWorkingHours
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TrainerId == trainerId && x.DayOfWeek == day);
        }

        // Çakışma var mı?
        public async Task<bool> HasConflict(int trainerId, DateTime startTime, int duration)
        {
            var endTime = startTime.AddMinutes(duration);

            return await _context.Appointments.AnyAsync(a =>
                a.TrainerId == trainerId &&
                a.Status != AppointmentStatus.Rejected &&
                startTime < a.StartTime.AddMinutes(a.Duration) &&
                endTime > a.StartTime
            );
        }

        // Hem çalışma saatine uyuyor mu hem çakışma yok mu?
        public async Task<bool> IsTrainerAvailable(int trainerId, DateTime startTime, int duration)
        {
            var wh = await GetWorkingHour(trainerId, startTime);
            if (wh == null) return false;

            var startT = TimeOnly.FromDateTime(startTime);
            var endT = startT.AddMinutes(duration);

            if (startT < wh.StartTime || endT > wh.EndTime)
                return false;

            return !await HasConflict(trainerId, startTime, duration);
        }

        // UI için uygun saatleri üret (her saat başı) — basit ve sağlam
        public async Task<List<TimeSpan>> GetAvailableHours(int trainerId, DateTime date, int durationMinutes)
        {
            var wh = await GetWorkingHour(trainerId, date);
            if (wh == null) return new List<TimeSpan>();

            var results = new List<TimeSpan>();

            // saat başı slot (08:00, 09:00, ...)
            var cursor = wh.StartTime;
            while (cursor.AddMinutes(durationMinutes) <= wh.EndTime)
            {
                var candidateStart = date.Date + cursor.ToTimeSpan();
                var ok = await IsTrainerAvailable(trainerId, candidateStart, durationMinutes);

                if (ok)
                    results.Add(cursor.ToTimeSpan());

                cursor = cursor.AddHours(1);
            }

            return results;
        }
    }
}
