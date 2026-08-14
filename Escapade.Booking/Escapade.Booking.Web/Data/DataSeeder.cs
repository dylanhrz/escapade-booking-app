using Escapade.Booking.Core.Entities;
using Microsoft.AspNetCore.Identity;
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
        
        var hasher = new PasswordHasher<User>();

        var user1 = new User 
        { 
            Id = 1, 
            UserName = "Wendy", 
            Email = "wendymartens19@gmail.com", 
            UserRoleId = 1 
        };
        
        user1.Password = hasher.HashPassword(user1, "ww123");
        
        
        var user2 = new User 
        { 
            Id = 2, 
            UserName = "DylanHourez", 
            Email = "dylanhourez@gmail.com", 
            UserRoleId = 1 
        };
        
        user2.Password = hasher.HashPassword(user2, "ww111");
        
        var users = new User[]
        {
            user1, user2
        };

        var bookings = new BookingEntity[]
        {
            new BookingEntity
            {
                Id = 1,
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