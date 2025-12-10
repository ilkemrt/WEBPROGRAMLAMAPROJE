using Microsoft.AspNetCore.Identity;

namespace SPORSALONUPROJE.Models
{
    public class Uye : IdentityUser
    {
        public string AdSoyad { get; set; }
        public DateTime UyeOlmaTarihi { get; set; } = DateTime.Now;

        public ICollection<Randevu> Randevular { get; set; }
        public string Rol { get; set; }
    }
}
