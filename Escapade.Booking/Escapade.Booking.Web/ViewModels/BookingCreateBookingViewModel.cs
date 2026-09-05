using System.ComponentModel.DataAnnotations;

namespace Escapade.Booking.Web.ViewModels;

public class BookingCreateBookingViewModel
{

    [Required(ErrorMessage = "Naam is verplicht")]
    [Display(Name = "Naam")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "E-mailadres is verplicht")]
    [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Telefoonnummer is verplicht")]
    [Phone(ErrorMessage = "Ongeldig telefoonnummer")]
    [Display(Name = "Telefoonnummer")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Aantal gasten is verplicht")]
    [Range(1, 10, ErrorMessage = "Aantal gasten moet tussen 1 en 10 liggen")]
    [Display(Name = "Aantal Gasten")]
    public int NumberOfGuests { get; set; } = 1;
    
    [Display(Name = "Opmerking")]
    public string? Comment { get; set; }
    
    [Required(ErrorMessage = "Selecteer een periode op de kalender")]
    [DataType(DataType.Date)]
    [Display(Name = "Aankomstdatum")]
    public DateTime? StartDate { get; set; }

    [Required(ErrorMessage = "Selecteer een periode op de kalender")]
    [DataType(DataType.Date)]
    [Display(Name = "Vertrekdatum")]
    public DateTime? EndDate { get; set; }

    public DateTime BookingCreated { get; set; }
}