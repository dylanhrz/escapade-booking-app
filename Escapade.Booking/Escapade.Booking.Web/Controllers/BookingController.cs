using System.Diagnostics;
using Escapade.Booking.Web.Data;
using Escapade.Booking.Web.Models;
using Escapade.Booking.Web.Services.Interfaces;
using Escapade.Booking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Escapade.Booking.Web.Controllers;

public class BookingController : Controller
{
    private readonly EscapadeDbContext _escapadeDbContext;
    private readonly IEmailService _emailService;

    public BookingController(EscapadeDbContext escapadeDbContext, IEmailService emailService)
    {
        _escapadeDbContext = escapadeDbContext;
        _emailService = emailService;
    }
    
    [HttpGet]
    public IActionResult CreateBooking()
    {

        BookingCreateBookingViewModel bookingCreateBookingViewModel = new BookingCreateBookingViewModel();

        return View(bookingCreateBookingViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBooking(BookingCreateBookingViewModel bookingCreateBookingViewModel)
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
            EndDate = bookingCreateBookingViewModel.EndDate!.Value,
        };
        
        _escapadeDbContext.Bookings.Add(booking);
        
        try
        {
            await _escapadeDbContext.SaveChangesAsync();
        }
        catch (DbUpdateException dbUpdateException)
        {
            Debug.WriteLine(dbUpdateException.Message);
            return View("Error", new ErrorViewModel());
        }
        
        var tracking = Url.Action(
            action: nameof(TrackBooking),
            controller: "Booking",
            values: new { token = booking.AccesToken },
            protocol: Request.Scheme
        );
        
        if (!string.IsNullOrEmpty(tracking))
        {
            await _emailService.SendBookingConfirmationAsync(booking.Email, booking.Name, tracking);
        }

        return RedirectToAction(nameof(BookingConfirmation), new { token = booking.AccesToken });
    }
    
    [HttpGet]
    public IActionResult BookingConfirmation(Guid token)
    {
        if (token == Guid.Empty)
        {
            return RedirectToAction(nameof(CreateBooking));
        }

        ViewBag.Token = token;
        
        return View();
    }

    //[HttpGet("booking/track/{token}")]
    public async Task<IActionResult> TrackBooking(Guid token)
    {
        var booking = await _escapadeDbContext.Bookings
            .FirstOrDefaultAsync(b => b.AccesToken == token);

        if (booking == null) return NotFound();

        BookingTrackBooking bookingTrackBooking = new BookingTrackBooking()
        {
            Name = booking.Name,
            Email = booking.Email,
            NumberOfGuests = booking.NumberOfGuests,
            BookingRequested = booking.Created,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            AccessToken = booking.AccesToken,
            Status = booking.Status.ToString()
        };

        return View(bookingTrackBooking);
    }
}