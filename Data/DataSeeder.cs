using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IConfiguration config, ILogger? logger = null)
        {
            // 1) Make sure the 5 well-known roles exist.
            var rolesCreated = 0;
            foreach (var roleName in RoleNames.All)
            {
                var exists = await context.UserRole.AnyAsync(r => r.RoleName == roleName);
                if (!exists)
                {
                    await context.UserRole.AddAsync(new Role { RoleName = roleName });
                    rolesCreated++;
                }
            }
            await context.SaveChangesAsync();
            if (rolesCreated > 0)
                logger?.LogInformation("Seeded {Count} new role(s).", rolesCreated);

            // 2) Make sure at least one Admin account exists, so someone can log in
            //    and start approving sign-ups / assigning roles. Uses DefaultAdmin
            //    settings from appsettings.json — CHANGE THIS PASSWORD after first login.
            var adminRole = await context.UserRole.FirstOrDefaultAsync(r => r.RoleName == RoleNames.Admin);
            if (adminRole == null)
            {
                logger?.LogWarning("Admin role not found after seeding roles — skipping default admin creation.");
                return;
            }

            var anyAdminExists = await context.Users.AnyAsync(u => u.RoleId == adminRole.RoleId);
            if (anyAdminExists)
            {
                logger?.LogInformation("An Admin account already exists — skipping default admin creation.");
                return;
            }

            var defaultUserName = config["DefaultAdmin:UserName"] ?? "admin";
            var defaultFullName = config["DefaultAdmin:FullName"] ?? "Administrator";
            var defaultEmail = config["DefaultAdmin:Email"] ?? "admin@prcs-logistics.local";
            var defaultPassword = config["DefaultAdmin:Password"] ?? "Admin@12345";
            var defaultPhone = config["DefaultAdmin:PhoneNumber"] ?? string.Empty;

            var admin = new User
            {
                UserName = defaultUserName,
                FullName = defaultFullName,
                Email = defaultEmail,
                PhoneNumber = defaultPhone,
                Password = BCrypt.Net.BCrypt.HashPassword(defaultPassword),
                Status = UserStatus.Active,
                RoleId = adminRole.RoleId,
                ApprovedBy = "system-seed",
                ApprovedAt = DateTime.UtcNow,
            };

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
            logger?.LogInformation(
                "Seeded default Admin account — username: '{UserName}', email: '{Email}'. Change the password after first login.",
                defaultUserName, defaultEmail);

        }
    }
}
