using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEBPROJE1.Models; // Modellerin olduğu yer


namespace WEBPROJE1.Data
{
    public class SporContext : IdentityDbContext<Uye>
    {
        public SporContext(DbContextOptions<SporContext> options) : base(options)
        {
        }

        public DbSet<Antrenor> Antrenorler { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }

    }
}
