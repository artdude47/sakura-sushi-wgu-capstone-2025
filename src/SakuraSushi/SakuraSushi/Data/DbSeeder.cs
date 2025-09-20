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
                db.AddRange(new Nigiri("Salmon", "Fresh salmon over vinegared rice", 3.50m),
                            new Nigiri("Tuna", "Fresh tuna over vinegared rice", 4.00m),
                            new Sashimi("Yellowtail", "Fresh yellowtail slices", 5.00m),
                            new Roll("California Roll", "Crab, avocado, cucumber", 6.50m),
                            new Roll("Spicy Tuna Roll", "Spicy tuna mix, cucumber", 7.00m));
                await db.SaveChangesAsync();
            }

            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            foreach (var r in new[] { "Owner", "Staff" }) if (!await roleMgr.RoleExistsAsync(r)) await roleMgr.CreateAsync(new IdentityRole(r));
            var adminEmail = "owner@sakura.local";
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userMgr.CreateAsync(admin, "ChangeMe1!");
                await userMgr.AddToRoleAsync(admin, "Owner");
            }
        }
    }
}
