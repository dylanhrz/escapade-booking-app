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

    public async Task SendNewBookingNotificationAsync(Entities.Booking booking, string detailsUrl)
    {
        var ownerEmail = _configuration["Booking:OwnerEmail"];
        if (string.IsNullOrWhiteSpace(ownerEmail)) return;

        var culture = new System.Globalization.CultureInfo("nl-BE");
        var nights = (booking.EndDate.Date - booking.StartDate.Date).Days;

        var subject = $"Nieuwe aanvraag: {booking.Name}, {booking.StartDate.ToString("d MMM", culture)} - {booking.EndDate.ToString("d MMM yyyy", culture)}";

        var body = $@"
        <h2>Nieuwe aanvraag van {WebUtility.HtmlEncode(booking.Name)}</h2>
        <p><strong>Periode:</strong> {booking.StartDate.ToString("d MMMM yyyy", culture)} t/m {booking.EndDate.ToString("d MMMM yyyy", culture)} ({nights} nachten)</p>
        <p><strong>Gasten:</strong> {booking.NumberOfGuests}</p>
        <p><strong>E-mail:</strong> {WebUtility.HtmlEncode(booking.Email)}</p>
        <p><strong>Telefoon:</strong> {WebUtility.HtmlEncode(booking.PhoneNumber)}</p>
        <p><strong>Opmerking:</strong> {WebUtility.HtmlEncode(booking.Comment ?? "-")}</p>
        <p><a href='{detailsUrl}'>Bekijk alle aanvragen</a></p>";

        // Gebruik een aparte mailserver voor de eigenaar als "OwnerSmtp" ingesteld is (bv. om te testen),
        // anders de gewone "Smtp"-instellingen.
        var smtpSection = string.IsNullOrWhiteSpace(_configuration["OwnerSmtp:Host"]) ? "Smtp" : "OwnerSmtp";

        await SendAsync(ownerEmail, subject, body, replyTo: booking.Email, smtpSection: smtpSection);
    }

    private async Task SendAsync(string toEmail, string subject, string body, string? replyTo = null, string smtpSection = "Smtp")
    {
        var smtp = _configuration.GetSection(smtpSection);

        using var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"] ?? "587"))
        {
            Credentials = new NetworkCredential(smtp["Username"], smtp["Password"]),
            EnableSsl = true
        };

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(smtp["FromAddress"]!, smtp["FromName"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);
        if (!string.IsNullOrWhiteSpace(replyTo)) mailMessage.ReplyToList.Add(replyTo);

        await client.SendMailAsync(mailMessage);
    }
}