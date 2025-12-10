using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SPORSALONUPROJE.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SPORSALONUPROJE.Data
{
    public class UygulamaDbContext : IdentityDbContext<Uye>
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options)
            : base(options)
        {
        }


        

        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<AntrenorHizmet> AntrenorHizmetleri { get; set; }
        public DbSet<AntrenorMusaitlik> AntrenorMusaitlikler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AntrenorHizmet - çoktan çoğa ilişki
            modelBuilder.Entity<AntrenorHizmet>()
                .HasKey(ah => new { ah.AntrenorId, ah.HizmetId });

            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Antrenor)
                .WithMany(a => a.AntrenorHizmetleri)
                .HasForeignKey(ah => ah.AntrenorId);

            modelBuilder.Entity<AntrenorHizmet>()
                .HasOne(ah => ah.Hizmet)
                .WithMany()
                .HasForeignKey(ah => ah.HizmetId);

            // AntrenorMusaitlik ilişkisi
            modelBuilder.Entity<AntrenorMusaitlik>()
                .HasOne(am => am.Antrenor)
                .WithMany(a => a.Musaitlikler)
                .HasForeignKey(am => am.AntrenorId);

            // Randevu ilişkileri
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Antrenor)
                .WithMany()
                .HasForeignKey(r => r.AntrenorId);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hizmet)
                .WithMany()
                .HasForeignKey(r => r.HizmetId);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Uye)
                .WithMany(u => u.Randevular)
                .HasForeignKey(r => r.UyeId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hizmet>()
                .Property(h => h.Ucret)
                .HasPrecision(10, 2); // toplam 10 hane, 2’si virgülden sonra

        }
    }
}
