using Escapade.Booking.Web.Data;
using Escapade.Booking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Escapade.Booking.Web.Controllers;

public class AdminController : Controller
{
    private readonly EscapadeDbContext _escapadeDbContext;

    public AdminController(EscapadeDbContext dbContext)
    {
        _escapadeDbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        string? adminName = HttpContext.Session.GetString("AdminName");
        string? adminId = HttpContext.Session.GetString("AdminId");

        if (string.IsNullOrEmpty(adminName) || string.IsNullOrEmpty(adminId))
        {
            return RedirectToAction("Login");
        }
        
        AdminIndexViewModel adminIndexViewModel = new ()
        {
            Bookings = _escapadeDbContext.Bookings
                .Select(b => new BaseViewModel()
            {
                Name = b.Name,
                Email = b.Email,
                PhoneNumber = b.PhoneNumber,
                NumberOfGuests = b.NumberOfGuests,
                Comment = b.Comment,
                StartDate = b.StartDate,
                EndDate = b.EndDate
            }).ToList()
            
        };
        
        return View(adminIndexViewModel);
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
        {
            return RedirectToAction("Index");
        }

        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(AdminLoginViewModel adminLoginViewModel)
    {
        if (!ModelState.IsValid) return View(adminLoginViewModel);

        var user = _escapadeDbContext.Users
            .Include(u => u.UserRole)
            .FirstOrDefault(u => u.Email == adminLoginViewModel.Email && u.Password == adminLoginViewModel.Password);

        if (user != null)
        {
            HttpContext.Session.SetString("AdminId", user.Id.ToString());
            HttpContext.Session.SetString("AdminName", user.UserName);

            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Ongeldige login gegevens");
        
        return View(adminLoginViewModel);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}