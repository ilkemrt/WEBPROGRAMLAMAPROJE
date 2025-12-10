using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SPORSALONUPROJE.Models;

namespace SPORSALONUPROJE.Data
{
    public static class VeriYukleyici
    {
        public static async Task VerileriYukleAsync(IServiceProvider hizmetSaglayici)
        {
            var userManager = hizmetSaglayici.GetRequiredService<UserManager<Uye>>();
            var roleManager = hizmetSaglayici.GetRequiredService<RoleManager<IdentityRole>>();

            // Roller
            string[] roller = { "Admin", "Uye" };

            foreach (var rol in roller)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            // Admin kullanıcı
            string adminEmail = "b231210085@sakarya.edu.tr";
            string adminSifre = "sau";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new Uye
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    AdSoyad = "Yönetici"
                };

                var sonuc = await userManager.CreateAsync(admin, adminSifre);
                if (sonuc.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
