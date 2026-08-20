using Escapade.Booking.Core.Entities;
using Microsoft.EntityFrameworkCore;
using BookingEntity = Escapade.Booking.Core.Entities.Booking;

namespace Escapade.Booking.Web.Data;

public static class DataSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var roles = new Role[]
        {
            new Role
            {
                Id = 1, 
                Name = "Admin"
            }
        };
        
        var users = new User[]
        { 
            new User
            {
                Id = 1, 
                UserName = "Wendy", 
                Email = "wendymartens19@gmail.com", 
                Password = "ww123",
                UserRoleId = 1 
            },
            new User
            {
                Id = 2, 
                UserName = "Dylan", 
                Email = "dylanhourez@gmail.com", 
                Password = "ww111",
                UserRoleId = 1
            }
        };

        var bookings = new BookingEntity[]
        {
            new BookingEntity
            {
                Id = 1,
                AccesToken = Guid.Parse("9f82d1a3-2c11-4e89-8012-3456789abcde"),
                Name = "Jean-Pierre Dubois",
                Email = "jp.dubois@cyclisme-club.fr",
                PhoneNumber = "+33 6 12 34 56 78",
                NumberOfGuests = 4,
                Comment = "Komen voor een wielerweek. Is er een veilige plek om de koersfietsen binnen te stallen?",
                StartDate = new DateTime(2026, 9, 15),
                EndDate = new DateTime(2026, 9, 22)
            },
            new BookingEntity
            {
                Id = 2,
                AccesToken = Guid.Parse("4a71b2e5-8d93-4f62-b138-028d71e95c1a"),
                Name = "Sophie Van De Velde",
                Email = "sophie.vdv@gmail.com",
                PhoneNumber = "+32 478 99 88 77",
                NumberOfGuests = 2,
                Comment = "Weekendje wandelen in de Ardennen.",
                StartDate = new DateTime(2026, 10, 2),
                EndDate = new DateTime(2026, 10, 5)
            }
        };
        
        modelBuilder.Entity<Role>().HasData(roles);
        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<BookingEntity>().HasData(bookings);
    }
}