namespace Escapade.Booking.Web.ViewModels;

public class BaseViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    public int NumberOfGuests { get; set; }
    public string Comment { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public Guid AccessToken { get; set; }
}