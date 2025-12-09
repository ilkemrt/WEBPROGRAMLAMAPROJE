namespace WEBPROJE1.Models
{
    public class Antrenor
    {
        public int Id { get; set; }
        public string TamAd { get; set; }
        public string UzmanlikAlani { get; set; } // Örn: Yoga, Kas Gelistirme
        public string? ResimUrl { get; set; }

        // Navigation Property: Antrenörün randevuları
        public ICollection<Randevu> Randevular { get; set; }

    }
}
