using Microsoft.EntityFrameworkCore;
using RoleEntity = Escapade.Booking.Core.Entities.Role;
using UserEntity = Escapade.Booking.Core.Entities.User;
using BookingEntity = Escapade.Booking.Core.Entities.Booking;

namespace Escapade.Booking.Web.Data;

public class EscapadeDbContext : DbContext
{
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BookingEntity> Bookings { get; set; }
    
    public EscapadeDbContext(DbContextOptions<EscapadeDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        DataSeeder.Seed(modelBuilder);
    } 
}