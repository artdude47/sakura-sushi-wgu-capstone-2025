using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SakuraSushi.Domain;
using static SakuraSushi.Domain.MenuItem;

namespace SakuraSushi.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<MenuItem>()
             .HasDiscriminator<string>("Discriminator")
             .HasValue<Nigiri>("Nigiri")
             .HasValue<Sashimi>("Sashimi")
             .HasValue<Roll>("Roll");

            b.Entity<MenuItem>().Property(p => p.Price).HasPrecision(10, 2);
            b.Entity<Reservation>().Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
