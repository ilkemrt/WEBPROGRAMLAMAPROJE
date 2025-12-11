using FitnessCenter.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessCenter.Web.Data
{
    public class SeedData
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager,
                                            UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "b231210085@sakarya.edu.tr";
            string adminPassword = "sau";

            // Rol kontrol
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("Member"))
                await roleManager.CreateAsync(new IdentityRole("Member"));

            // Admin user var mı kontrol
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
