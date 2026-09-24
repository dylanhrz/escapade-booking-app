using System.Globalization;
using Escapade.Booking.Web.Data;
using Escapade.Booking.Web.Services;
using Escapade.Booking.Web.Services.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddDbContext<EscapadeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.Configure<RouteOptions>(options => 
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

var supportedCultures = new[]
{
    new CultureInfo("nl"),
    new CultureInfo("fr"),
    new CultureInfo("en")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("nl"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "admin",
    pattern: "admin",
    defaults: new { controller = "Admin", action = "Login" });

app.MapControllerRoute(
    name: "createBooking",
    pattern: "booking/request",
    defaults: new { controller = "Booking", action = "CreateBooking" });

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();