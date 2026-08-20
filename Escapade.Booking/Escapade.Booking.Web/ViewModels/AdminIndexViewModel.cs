namespace Escapade.Booking.Web.ViewModels;

public class AdminIndexViewModel
{
    public ICollection<BaseViewModel> UpcomingBookings { get; set; } = new List<BaseViewModel>();
    public ICollection<BaseViewModel> PastBookings { get; set; } = new List<BaseViewModel>();
}