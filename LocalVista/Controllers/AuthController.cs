using LocalVista.DTOs;
using LocalVista.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalVista.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken ct)
    {
        var result = await auth.LoginAsync(request, ct);
        if (!result.Ok)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await auth.LogoutAsync();
        return NoContent();
    }

    [HttpGet("me")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthUserDto?>> Me()
    {
        // Guests are allowed — return null with 200 so the SPA does not treat this as an error.
        var user = await auth.GetCurrentUserAsync(User);
        return Ok(user);
    }
}
