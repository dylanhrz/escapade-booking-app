namespace Escapade.Booking.Web.Services.Interfaces;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(string toEmail, string name, string tracking);
}