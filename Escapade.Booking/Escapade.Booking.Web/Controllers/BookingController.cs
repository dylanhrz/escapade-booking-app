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
    public IActionResult CreateBooking()
    {

        BookingCreateBookingViewModel bookingCreateBookingViewModel = new BookingCreateBookingViewModel();

        return View(bookingCreateBookingViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateBooking(BookingCreateBookingViewModel bookingCreateBookingViewModel)
    {
        if (bookingCreateBookingViewModel.StartDate.HasValue && bookingCreateBookingViewModel.EndDate.HasValue)
        {
            if (bookingCreateBookingViewModel.EndDate.Value <= bookingCreateBookingViewModel.StartDate.Value)
            {
                ModelState.AddModelError("EndDate", "Vertrekdatum moet na de aankomstdatum liggen.");
            }
        }
        
        if (!ModelState.IsValid)
        {
            return View(bookingCreateBookingViewModel);
        }

        var booking = new Core.Entities.Booking()
        {
            Name = bookingCreateBookingViewModel.Name,
            Email = bookingCreateBookingViewModel.Email,
            PhoneNumber = bookingCreateBookingViewModel.PhoneNumber,
            NumberOfGuests = bookingCreateBookingViewModel.NumberOfGuests,
            Comment = bookingCreateBookingViewModel.Comment,
            StartDate = bookingCreateBookingViewModel.StartDate!.Value,
            EndDate = bookingCreateBookingViewModel.EndDate!.Value
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

    //[HttpGet("booking/track/{token}")]
    public async Task<IActionResult> TrackBooking(Guid token)
    {
        var booking = await _escapadeDbContext.Bookings
            .FirstOrDefaultAsync(b => b.AccesToken == token);

        if (booking == null) return NotFound();

        BookingTrackBooking bookingTrackBooking = new BookingTrackBooking()
        {
            Email = booking.Email,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            AccessToken = booking.AccesToken,
            Status = booking.Status.ToString()
        };

        return View(bookingTrackBooking);
    }
}