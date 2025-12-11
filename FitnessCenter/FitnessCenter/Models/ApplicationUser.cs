using Microsoft.AspNetCore.Identity;

namespace FitnessCenter.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // AI kullanımı için opsiyonel
        public int? Height { get; set; }
        public int? Weight { get; set; }
    }
}