using Escapade.Booking.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Escapade.Booking.Web.Controllers;

public class BookingController : Controller
{
    
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
        if (bookingNewBookingViewModel.EndDate <= bookingNewBookingViewModel.StartDate)
        {
            ModelState.AddModelError("EndDate", "Vertrekdatum moet na de aankomstdatum liggen.");
        }

        return View();
    }

    
    
}