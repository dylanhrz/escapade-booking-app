namespace Escapade.Booking.Web.ViewModels;

public class BookingTrackBooking
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int NumberOfGuests { get; set; }
    public DateTime BookingRequested { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid AccessToken { get; set; }
    public string Status { get; set; }
}