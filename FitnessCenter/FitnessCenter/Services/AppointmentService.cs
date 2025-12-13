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
        public async Task<List<TimeSpan>> GetAvailableHours(
        int trainerId,
        DateTime date,
        int serviceDurationMinutes)
        {
            var result = new List<TimeSpan>();

            // geçmiş gün engeli
            if (date.Date < DateTime.Today)
                return result;

            var workingHour = await GetWorkingHour(trainerId, date);
            if (workingHour == null)
                return result;

            // sabit blok: 1 saat
            var slotStep = TimeSpan.FromHours(1);

            var cursor = workingHour.StartTime;

            while (true)
            {
                var start = date.Date + cursor.ToTimeSpan();
                var end = start.AddMinutes(serviceDurationMinutes);

                // çalışma saatini aşıyor mu?
                if (end.TimeOfDay > workingHour.EndTime.ToTimeSpan())
                    break;

                // bugünkü geçmiş saat engeli
                if (date.Date == DateTime.Today && start <= DateTime.Now)
                {
                    cursor = cursor.AddHours(1);
                    continue;
                }

                // çakışma var mı?
                var conflict = await HasConflict(trainerId, start, serviceDurationMinutes);
                if (!conflict)
                    result.Add(cursor.ToTimeSpan());

                cursor = cursor.Add(slotStep);
            }

            return result;
        }

    }
}
