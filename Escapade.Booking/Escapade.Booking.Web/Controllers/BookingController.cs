using System.Diagnostics;
using Escapade.Booking.Web.Data;
using Escapade.Booking.Web.Models;
using Escapade.Booking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Escapade.Booking.Web.Controllers;

public class BookingController : Controller
{
    private readonly EscapadeDbContext _escapadeDbContext;

    public BookingController(EscapadeDbContext escapadeDbContext)
    {
        _escapadeDbContext = escapadeDbContext;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult NewBooking()
    {

        BookingNewBookingViewModel bookingNewBookingViewModel = new BookingNewBookingViewModel();

        return View(bookingNewBookingViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult NewBooking(BookingNewBookingViewModel bookingNewBookingViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(bookingNewBookingViewModel);
        }
        
        if (bookingNewBookingViewModel.EndDate <= bookingNewBookingViewModel.StartDate)
        {
            ModelState.AddModelError("EndDate", "Vertrekdatum moet na de aankomstdatum liggen.");
        }

        var booking = new Core.Entities.Booking()
        {
            Name = bookingNewBookingViewModel.Name,
            Email = bookingNewBookingViewModel.Email,
            PhoneNumber = bookingNewBookingViewModel.PhoneNumber,
            NumberOfGuests = bookingNewBookingViewModel.NumberOfGuests,
            Comment = bookingNewBookingViewModel.Comment,
            StartDate = bookingNewBookingViewModel.StartDate,
            EndDate = bookingNewBookingViewModel.EndDate
        };
        
        _escapadeDbContext.Bookings.Add(booking);
        
        try
        {
            _escapadeDbContext.SaveChanges();
        }
        catch (DbUpdateException dbUpdateException)
        {
            Debug.WriteLine(dbUpdateException.Message);
            return View("Error", new ErrorViewModel());
        }

        return RedirectToAction("Index");
    }

    
    
}