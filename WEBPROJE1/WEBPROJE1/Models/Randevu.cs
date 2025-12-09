namespace WEBPROJE1.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public DateTime RandevuTarihi { get; set; }
        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

        // Randevu Durumu: Enum sayesinde veritabanında sadece sayı (int) tutulur.
        public RandevuDurumu Durum { get; set; }

        // --- Foreign Keys (Yabancı Anahtarlar) ve Navigasyon Özellikleri ---

        // Uye İlişkisi
        public string UyeId { get; set; } // Uye.Id alanından miras alır
        public Uye Uye { get; set; }

        // Antrenör İlişkisi
        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }

        // Hizmet İlişkisi
        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }
    }

    public enum RandevuDurumu
    {
        Beklemede,   // 0
        Onaylandi,   // 1
        IptalEdildi, // 2
        Tamamlandi   // 3
    }
}
