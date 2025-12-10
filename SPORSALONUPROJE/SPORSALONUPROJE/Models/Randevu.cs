namespace SPORSALONUPROJE.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        public string UyeId { get; set; }
        public Uye Uye { get; set; }

        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }

        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }

        public DateTime BaslangicZamani { get; set; }
        public bool OnaylandiMi { get; set; } = false;
        public bool IptalEdildiMi { get; set; } = false;
    }
}
