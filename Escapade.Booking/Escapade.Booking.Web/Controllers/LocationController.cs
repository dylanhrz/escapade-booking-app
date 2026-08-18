using Microsoft.AspNetCore.Mvc;

namespace Escapade.Booking.Web.Controllers;

public class LocationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}