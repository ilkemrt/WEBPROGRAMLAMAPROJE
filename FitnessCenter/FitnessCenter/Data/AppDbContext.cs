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
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Trainer -> Service (1-n)
            builder.Entity<Trainer>()
                .HasOne(t => t.Service)
                .WithMany(s => s.Trainers)
                .HasForeignKey(t => t.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment -> Trainer
            builder.Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TrainerId);

            // Appointment -> Member
            builder.Entity<Appointment>()
                .HasOne(a => a.Member)
                .WithMany()
                .HasForeignKey(a => a.MemberId);

            // Appointment -> Service
            builder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId);
        }
    }
}
