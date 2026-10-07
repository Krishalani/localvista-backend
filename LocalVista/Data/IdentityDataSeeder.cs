using LocalVista.Data.Entities;
using LocalVista.Services;
using Microsoft.AspNetCore.Identity;

namespace LocalVista.Data;

/// <summary>
/// Seeds ASP.NET Core Identity Admin role/user only.
/// Does not insert or modify Categories / Attractions / AttractionImages (SQL seed owns those).
/// </summary>
public static class IdentityDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        if (!await roleManager.RoleExistsAsync(AuthService.AdminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(AuthService.AdminRole));
        }

        var username = config["AdminSeed:Username"] ?? "manager";
        var password = config["AdminSeed:Password"] ?? "Manager123";
        var displayName = config["AdminSeed:DisplayName"] ?? "Nimal (demo admin)";

        var existing = await userManager.FindByNameAsync(username);
        if (existing is not null)
        {
            if (!await userManager.IsInRoleAsync(existing, AuthService.AdminRole))
            {
                await userManager.AddToRoleAsync(existing, AuthService.AdminRole);
            }

            return;
        }

        var user = new ApplicationUser
        {
            UserName = username,
            Email = $"{username}@localvista.local",
            EmailConfirmed = true,
            DisplayName = displayName,
        };

        var create = await userManager.CreateAsync(user, password);
        if (!create.Succeeded)
        {
            var errors = string.Join("; ", create.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to seed admin user: {errors}");
        }

        await userManager.AddToRoleAsync(user, AuthService.AdminRole);
    }
}
