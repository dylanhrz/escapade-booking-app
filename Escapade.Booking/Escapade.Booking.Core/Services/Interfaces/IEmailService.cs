namespace Escapade.Booking.Core.Services.Interfaces;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(string toEmail, string name, string tracking);
}