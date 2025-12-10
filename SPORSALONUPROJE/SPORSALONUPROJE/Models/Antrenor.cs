namespace SPORSALONUPROJE.Models
{
    public class Antrenor
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Aciklama { get; set; }

        public ICollection<AntrenorHizmet> AntrenorHizmetleri { get; set; }
        public ICollection<AntrenorMusaitlik> Musaitlikler { get; set; }
    }
}
