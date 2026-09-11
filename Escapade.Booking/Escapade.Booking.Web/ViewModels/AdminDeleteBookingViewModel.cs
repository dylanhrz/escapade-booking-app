using Microsoft.AspNetCore.Mvc;

namespace Escapade.Booking.Web.ViewModels;

public class AdminDeleteBookingViewModel
{
    [HiddenInput]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
}