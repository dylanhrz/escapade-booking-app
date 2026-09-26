namespace Escapade.Booking.Core.Services.Models.RequestModels;

public class BookingCreateRequestModel
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int NumberOfGuests { get; set; }
    public string? Comment { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}