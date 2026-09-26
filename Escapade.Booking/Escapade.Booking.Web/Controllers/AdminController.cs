using Escapade.Booking.Core.Services.Interfaces;
using Escapade.Booking.Core.Services.Models.RequestModels;
using Escapade.Booking.Web.Models;
using Escapade.Booking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Escapade.Booking.Web.Controllers;

public class AdminController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;

    public AdminController(IBookingService bookingService, IUserService userService)
    {
        _bookingService = bookingService;
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? adminName = HttpContext.Session.GetString("AdminName");
        string? adminId = HttpContext.Session.GetString("AdminId");

        if (string.IsNullOrEmpty(adminName) || string.IsNullOrEmpty(adminId))
        {
            return RedirectToAction("Login");
        }

        var today = DateTime.Today;
        var result = await _bookingService.GetAllAsync();
        
        if (!result.IsSuccess)
        {
            return View("Error", new ErrorViewModel());
        }

        var bookings = result.Items.Select(b => new BaseViewModel()
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
    public async Task<IActionResult> Login(AdminLoginViewModel adminLoginViewModel)
    {
        if (!ModelState.IsValid) return View(adminLoginViewModel);
        
        var requestModel = new UserLoginRequestModel
        {
            Email = adminLoginViewModel.Email,
            Password = adminLoginViewModel.Password
        };
        
        var result = await _userService.LoginAsync(requestModel);

        if (result.IsSuccess)
        {
            var user = result.Items.First();
            HttpContext.Session.SetString("AdminId", user.Id.ToString());
            HttpContext.Session.SetString("AdminName", user.UserName);

            return RedirectToAction("Index");
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error);
        }
        
        return View(adminLoginViewModel);
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookingService.GetByIdAsync(id);
        
        if (!result.IsSuccess) return NotFound();
        
        var booking = result.Items.First();
        
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
    public async Task<IActionResult> ApproveBooking(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
        {
            return RedirectToAction("Login");
        }

        var result = await _bookingService.ApproveBookingAsync(id);
        
        if (!result.IsSuccess)
        {
            return NotFound();
        }

        TempData["Message"] = "Boeking is succesvol goedgekeurd!";
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(AdminDeleteBookingViewModel adminDeleteBookingViewModel)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
        {
            return RedirectToAction("Login");
        }
        
        var result = await _bookingService.DeleteBookingAsync(adminDeleteBookingViewModel.Id);

        if (!result.IsSuccess)
        {
            return View("Error", new ErrorViewModel());
        }

        TempData["Message"] = "Boeking is succesvol verwijderd!";
        return RedirectToAction("Index");
    }
}