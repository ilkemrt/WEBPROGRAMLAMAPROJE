using SPORSALONUPROJE.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPORSALONUPROJE.ViewModels
{
    public class AntrenorFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; }

        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        [Display(Name = "Hizmetler")]
        public List<int> SecilenHizmetler { get; set; }

        public List<Hizmet> MevcutHizmetler { get; set; }
    }
}
