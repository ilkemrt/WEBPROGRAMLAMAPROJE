using Microsoft.AspNetCore.Identity;
namespace WEBPROJE1.Models

{
    public class Uye : IdentityUser
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public DateTime DogumTarihi { get; set; }
        public ICollection<Randevu> Randevular { get; set; }
    }
}
