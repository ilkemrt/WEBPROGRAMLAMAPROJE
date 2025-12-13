using FitnessCenter.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenter.Web.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerWorkingHour> TrainerWorkingHours { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // -------------------------
            // Trainer -> Service (N:1)
            // -------------------------
            builder.Entity<Trainer>()
                .HasOne(t => t.Service)
                .WithMany(s => s.Trainers)
                .HasForeignKey(t => t.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Trainer -> WorkingHours (1:N)
            // -------------------------
            builder.Entity<TrainerWorkingHour>()
                .HasOne(w => w.Trainer)
                .WithMany(t => t.WorkingHours)
                .HasForeignKey(w => w.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------------------------
            // Appointment -> Trainer (N:1)
            // -------------------------
            builder.Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------------------------
            // Appointment -> Member (N:1)
            // -------------------------
            builder.Entity<Appointment>()
                .HasOne(a => a.Member)
                .WithMany()
                .HasForeignKey(a => a.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Appointment -> Service (N:1)
            // -------------------------
            builder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
