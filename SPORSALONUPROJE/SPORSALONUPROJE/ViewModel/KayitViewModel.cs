using System.ComponentModel.DataAnnotations;

namespace SPORSALONUPROJE.ViewModels
{
    public class KayitViewModel
    {
        [Required]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; }

        [Required]
        [EmailAddress]
        public string Eposta { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [DataType(DataType.Password)]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor.")]
        [Display(Name = "Şifre Tekrar")]
        public string SifreTekrar { get; set; }
    }
}
