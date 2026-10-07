using LocalVista.Data.Entities;
using LocalVista.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LocalVista.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : IAuthService
{
    public const string AdminRole = "Admin";

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return new LoginResponseDto(false, Error: "Username and password are required.");
        }

        var user = await userManager.FindByNameAsync(request.Username.Trim());
        if (user is null)
        {
            return new LoginResponseDto(false, Error: "Invalid admin username or password.");
        }

        var result = await signInManager.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: true,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return new LoginResponseDto(false, Error: "Invalid admin username or password.");
        }

        if (!await userManager.IsInRoleAsync(user, AdminRole))
        {
            await signInManager.SignOutAsync();
            return new LoginResponseDto(false, Error: "Only administrators can sign in.");
        }

        return new LoginResponseDto(true, ToDto(user));
    }

    public Task LogoutAsync() => signInManager.SignOutAsync();

    public async Task<AuthUserDto?> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var user = await userManager.GetUserAsync(principal);
        if (user is null || !await userManager.IsInRoleAsync(user, AdminRole))
        {
            return null;
        }

        return ToDto(user);
    }

    private static AuthUserDto ToDto(ApplicationUser user) =>
        new(user.UserName ?? string.Empty, user.DisplayName, AdminRole);
}
