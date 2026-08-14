using Microsoft.EntityFrameworkCore;

namespace Escapade.Booking.Web.Data;

public class EscapadeDbContext : DbContext
{
    
    public DbSet<Core.Entities.Booking> Bookings { get; set; }
    
    public EscapadeDbContext(DbContextOptions<EscapadeDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // DataSeeder.Seed(modelBuilder);
    } 
}