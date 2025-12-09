using Microsoft.AspNetCore.Identity;
using WEBPROJE1.Models;
namespace WEBPROJE1.Data
{
    public class DbInitializer
    {
        public static void Baslat(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // UserManager ve RoleManager servislerini çağırıyoruz
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<Uye>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                // 1. ROLLERİ OLUŞTUR (Admin ve Uye)
                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                }

                if (!roleManager.RoleExistsAsync("Uye").Result)
                {
                    roleManager.CreateAsync(new IdentityRole("Uye")).Wait();
                }

                
                var adminEmail = "b231210085@sakarya.edu.tr"; // admin email

                var adminUser = userManager.FindByEmailAsync(adminEmail).Result;

                if (adminUser == null)
                {
                    var newAdmin = new Uye()
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        Ad = "Sistem",
                        Soyad = "Yöneticisi",
                        EmailConfirmed = true,
                        DogumTarihi = DateTime.Now.AddYears(-20)
                    };

                   
                    var result = userManager.CreateAsync(newAdmin, "sau").Result;

                    if (result.Succeeded)
                    {
                        
                        userManager.AddToRoleAsync(newAdmin, "Admin").Wait();
                    }
                }
            }
        }

    }
}
