using Escapade.Booking.Web.Services.Interfaces;

namespace Escapade.Booking.Web.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }
    
    public Task SendBookingConfirmationAsync(string toEmail, string name, string tracking)
    {
        var subject = "Bevestiging van je boeking - L'Escapade Ardennaise";
        
        var body = $@"
            <h2>Beste {name},</h2>
            <p>Bedankt voor je reservatieaanvraag!</p>
            <p>Je kunt de status van je boeking en je details op elk moment bekijken via de onderstaande link:</p>
            <p><a href='{tracking}' style='padding: 10px 20px; background-color: #0077ff; color: white; text-decoration: none;'>Bekijk je boeking</a></p>
            <p>Lukt de knop niet? Kopieer dan deze link naar je browser:<br>{tracking}</p>";
        
        _logger.LogInformation("[EMAIL STUB] Mail verzonden naar {Email} met link: {TrackingUrl}", toEmail, tracking);
        
        return Task.CompletedTask;
    }
}