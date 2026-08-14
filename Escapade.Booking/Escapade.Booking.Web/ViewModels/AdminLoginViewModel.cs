using System.ComponentModel.DataAnnotations;

namespace Escapade.Booking.Web.ViewModels;

public class AdminLoginViewModel
{
    [Required(ErrorMessage = "Email is verplicht")]
    [EmailAddress(ErrorMessage = "Ongeldig emailadres")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Wachtwoord is verplicht")]
    [Display(Name = "Wachtwoord")]
    public string Password { get; set; } = string.Empty;
}