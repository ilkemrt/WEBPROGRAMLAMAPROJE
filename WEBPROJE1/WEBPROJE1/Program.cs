using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEBPROJE1.Data;
using WEBPROJE1.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SporContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<Uye, IdentityRole>(options =>
{
    // Þifre kurallarý (Öðrenci projesi olduðu için gevþetebiliriz)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3; // Þifre en az 3 karakter olsun yeter
})
.AddEntityFrameworkStores<SporContext>()
.AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    
pattern: "{controller=Home}/{action=Index}/{id?}")
  
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    // DbInitializer sýnýfýný kullanarak verileri basýyoruz
    WEBPROJE1.Data.DbInitializer.Baslat(app);
}

app.Run();
