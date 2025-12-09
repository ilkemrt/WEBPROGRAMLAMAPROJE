namespace WEBPROJE1.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int SureDakika { get; set; }
        public decimal Ucret { get; set; }

        // Navigation Property: Hizmetin randevuları
        public ICollection<Randevu>? Randevular { get; set; }

    }
}
