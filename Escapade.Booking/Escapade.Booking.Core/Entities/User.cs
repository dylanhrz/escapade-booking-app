namespace Escapade.Booking.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public int? UserRoleId { get; set; }
    public Role UserRole { get; set; }
}