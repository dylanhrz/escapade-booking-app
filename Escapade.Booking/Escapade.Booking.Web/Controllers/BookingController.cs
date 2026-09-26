using System.Diagnostics;
using Escapade.Booking.Core.Services.Interfaces;
using Escapade.Booking.Core.Services.Models.RequestModels;
using Escapade.Booking.Web.Models;
using Escapade.Booking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Escapade.Booking.Web.Controllers;

public class BookingController : Controller
{
    private readonly IBookingService _bookingService;
    private readonly IEmailService _emailService;

    public BookingController(IBookingService bookingService, IEmailService emailService)
    {
        _bookingService = bookingService;
        _emailService = emailService;
    }
    
    [HttpGet]
    public IActionResult CreateBooking()
    {
        BookingCreateBookingViewModel bookingCreateBookingViewModel = new ();
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

        var requestModel = new BookingCreateRequestModel
        {
            Name = bookingCreateBookingViewModel.Name,
            Email = bookingCreateBookingViewModel.Email,
            PhoneNumber = bookingCreateBookingViewModel.PhoneNumber,
            NumberOfGuests = bookingCreateBookingViewModel.NumberOfGuests,
            Comment = bookingCreateBookingViewModel.Comment,
            StartDate = bookingCreateBookingViewModel.StartDate!.Value,
            EndDate = bookingCreateBookingViewModel.EndDate!.Value
        };
        
        var result = await _bookingService.CreateBookingAsync(requestModel);

        if (!result.IsSuccess)
        {
            return View("Error", new ErrorViewModel());
        }
        
        var booking = result.Items.First();

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

    [HttpGet]
    public async Task<IActionResult> TrackBooking(Guid token)
    {
        var result = await _bookingService.GetByTokenAsync(token);
        
        if (!result.IsSuccess)
        {
            return NotFound();
        }
        
        var booking = result.Items.First();

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