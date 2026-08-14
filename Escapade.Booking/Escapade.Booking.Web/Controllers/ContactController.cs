using Microsoft.AspNetCore.Mvc;

namespace Escapade.Booking.Web.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}