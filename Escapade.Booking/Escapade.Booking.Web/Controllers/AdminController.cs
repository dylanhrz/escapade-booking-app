using System.Diagnostics;
using Escapade.Booking.Web.Data;
using Escapade.Booking.Web.Models;
using Escapade.Booking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Escapade.Booking.Web.Controllers;

public class AdminController : Controller
{
    private readonly EscapadeDbContext _escapadeDbContext;

    public AdminController(EscapadeDbContext escapadeDbContext)
    {
        _escapadeDbContext = escapadeDbContext;
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

        var today = DateTime.Today;

        var bookings = _escapadeDbContext.Bookings
            .Select(b => new BaseViewModel()
            {
                Id = b.Id,
                Name = b.Name,
                Email = b.Email,
                PhoneNumber = b.PhoneNumber,
                NumberOfGuests = b.NumberOfGuests,
                Comment = b.Comment,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                AccessToken = b.AccesToken
            }).ToList();
        
        AdminIndexViewModel adminIndexViewModel = new()
        {
            UpcomingBookings = bookings
                .Where(b => b.EndDate >= today)
                .OrderBy(b => b.StartDate)
                .ToList(),

            PastBookings = bookings
                .Where(b => b.EndDate < today)
                .OrderByDescending(b => b.StartDate)
                .ToList()
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
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var booking = _escapadeDbContext.Bookings.FirstOrDefault(p => p.Id == id);

        if (booking == null) return NotFound();

        AdminDeleteBookingViewModel adminDeleteBookingViewModel = new()
        {
            Id = booking.Id,
            Name = booking.Name,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate
        };

        return View(adminDeleteBookingViewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(AdminDeleteBookingViewModel adminDeleteBookingViewModel)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
        {
            return RedirectToAction("Login");
        }
        
        var booking = _escapadeDbContext.Bookings.FirstOrDefault(p => p.Id == adminDeleteBookingViewModel.Id);
        
        if (booking == null) return NotFound();
    
        _escapadeDbContext.Bookings.Remove(booking);

        try
        {
            _escapadeDbContext.SaveChanges();
            TempData["Message"] = $"Boeking voor '{booking.Name}' ({booking.StartDate:dd/MM/yyyy} - {booking.EndDate:dd/MM/yyyy}) is verwijderd!";
        }
        catch (DbUpdateException ex)
        {
            Debug.WriteLine(ex.Message);
            return View("Error", new ErrorViewModel());
        }

        return RedirectToAction("Index");
    }
}