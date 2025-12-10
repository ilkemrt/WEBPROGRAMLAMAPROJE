using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SPORSALONUPROJE.Data;
using SPORSALONUPROJE.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VarsayilanBaglanti")));


builder.Services.AddIdentity<Uye, IdentityRole>()
    .AddEntityFrameworkStores<UygulamaDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
});


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






// Seed iþlemi burada yapýlmalý
using (var scope = app.Services.CreateScope())
{
    var hizmetSaglayici = scope.ServiceProvider;
    await VeriYukleyici.VerileriYukleAsync(hizmetSaglayici);
}

app.Run();
