namespace SPORSALONUPROJE.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string Ad { get; set; } // Fitness, Yoga, Pilates...
        public TimeSpan Sure { get; set; }
        public decimal Ucret { get; set; }
    }
}
