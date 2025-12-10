using SPORSALONUPROJE.Models;
using System.ComponentModel.DataAnnotations;

namespace SPORSALONUPROJE.ViewModels
{
    public class RandevuFormViewModel
    {
        [Required]
        public int AntrenorId { get; set; }

        [Required]
        public int HizmetId { get; set; }

        [Required]
        public DateTime BaslangicZamani { get; set; }

        public List<Antrenor> Antrenorler { get; set; }
        public List<Hizmet> Hizmetler { get; set; }
    }
}
