using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SakuraSushi.Domain;
using static SakuraSushi.Domain.MenuItem;

namespace SakuraSushi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();

            if (!await db.MenuItems.AnyAsync())
            {
                var seedItems = new MenuItem[]
                {
                    // Nigiri
                    new Nigiri("Salmon",        "Fresh salmon over vinegared rice", 3.50m),
                    new Nigiri("Tuna",          "Fresh tuna over vinegared rice",   4.00m),
                    new Nigiri("Eel",           "Unagi with sweet tare glaze",      4.25m),
                    new Nigiri("Shrimp",        "Boiled ebi over rice",             3.25m),
                    new Nigiri("Scallop",       "Hotate, lightly torched",          4.75m),

                    // Sashimi
                    new Sashimi("Yellowtail",   "Hamachi sashimi slices",           5.00m),
                    new Sashimi("Salmon",       "Thick-cut salmon sashimi",         5.25m),
                    new Sashimi("Tuna",         "Lean maguro sashimi",              5.50m),

                    // Rolls
                    new Roll("California Roll", "Crab, avocado, cucumber",          6.50m),
                    new Roll("Spicy Tuna Roll", "Spicy tuna mix, cucumber",         7.00m),
                    new Roll("Rainbow Roll",    "Assorted fish over California",    12.00m),
                    new Roll("Dragon Roll",     "Eel, cucumber, avocado on top",    13.00m),
                    new Roll("Veggie Roll",     "Avocado, cucumber, carrot",        6.00m),
                    new Roll("Philadelphia",    "Salmon, cream cheese, cucumber",   8.50m),
                };

                db.AddRange(seedItems);
                await db.SaveChangesAsync();
            }

            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var r in new[] { "Owner", "Staff" })
            {
                if (!await roleMgr.RoleExistsAsync(r))
                    await roleMgr.CreateAsync(new IdentityRole(r));
            }

            // Owner
            var ownerEmail = "owner@sakura.local";
            var owner = await userMgr.FindByEmailAsync(ownerEmail);
            if (owner == null)
            {
                owner = new ApplicationUser { UserName = ownerEmail, Email = ownerEmail, EmailConfirmed = true };
                await userMgr.CreateAsync(owner, "ChangeMe1!");
                await userMgr.AddToRoleAsync(owner, "Owner");
            }

            // Staff
            var staffEmail = "staff@sakura.local";
            var staff = await userMgr.FindByEmailAsync(staffEmail);
            if (staff == null)
            {
                staff = new ApplicationUser { UserName = staffEmail, Email = staffEmail, EmailConfirmed = true };
                await userMgr.CreateAsync(staff, "ChangeMe1!");
                await userMgr.AddToRoleAsync(staff, "Staff");
            }

            if (!await db.Reservations.AnyAsync())
            {
                var nowLocal = DateTimeOffset.Now;

                var sample = new[]
                {
                    Reservation.Create("Bobby Test", 2, nowLocal.AddDays(1).AddHours(18 - nowLocal.Hour).AddMinutes(-nowLocal.Minute), SeatType.SushiBar, "555-0101"),
                    Reservation.Create("Test Person", 4, nowLocal.AddDays(2).AddHours(19 - nowLocal.Hour).AddMinutes(-nowLocal.Minute), SeatType.Table, "555-0202"),
                    Reservation.Create("Billy Test",     3, nowLocal.AddDays(3).AddHours(20 - nowLocal.Hour).AddMinutes(-nowLocal.Minute), SeatType.Table, "555-0303"),
                    Reservation.Create("Kim Test",      5, nowLocal.AddDays(5).AddHours(18 - nowLocal.Hour).AddMinutes(-nowLocal.Minute), SeatType.SushiBar, "555-0404"),
                };

                db.AddRange(sample);
                await db.SaveChangesAsync();
            }
        }
    }
}
