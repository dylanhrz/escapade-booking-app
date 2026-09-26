using System.Net;
using System.Net.Mail;
using Escapade.Booking.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Escapade.Booking.Core.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task SendBookingConfirmationAsync(string toEmail, string name, string tracking)
    {
        var subject = "Bevestiging van je boeking - L'Escapade Ardennaise";
        
        var body = $@"
            <h2>Beste {name},</h2>
            <p>Bedankt voor je reservatieaanvraag!</p>
            <p>Je kunt de status van je boeking en je details op elk moment bekijken via de onderstaande link:</p>
            <p><a href='{tracking}' style='padding: 10px 20px; background-color: #0077ff; color: white; text-decoration: none;'>Bekijk je boeking</a></p>
            <p>Lukt de knop niet? Kopieer dan deze link naar je browser:<br>{tracking}</p>";
        
        var host = _configuration["Smtp:Host"];
        var port = int.Parse(_configuration["Smtp:Port"] ?? "587");
        var username = _configuration["Smtp:Username"];
        var password = _configuration["Smtp:Password"];
        var fromAddress = _configuration["Smtp:FromAddress"];
        var fromName = _configuration["Smtp:FromName"];

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromAddress!, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        
        mailMessage.To.Add(toEmail);
        
        await client.SendMailAsync(mailMessage);
    }
}