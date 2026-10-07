using System.Security.Claims;
using LocalVista.DTOs;

namespace LocalVista.Services;

public interface IAttractionService
{
    Task<IReadOnlyList<AttractionDto>> ListAsync(string? search, IReadOnlyList<string>? categories, CancellationToken ct);
    Task<AttractionDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<(AttractionDto? Dto, string? Error)> CreateAsync(AttractionWriteDto request, CancellationToken ct);
    Task<(AttractionDto? Dto, string? Error)> UpdateAsync(int id, AttractionWriteDto request, CancellationToken ct);
    Task<(bool Ok, string? Error)> DeleteAsync(int id, CancellationToken ct);
}

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> ListAsync(CancellationToken ct);
}

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct);
    Task LogoutAsync();
    Task<AuthUserDto?> GetCurrentUserAsync(ClaimsPrincipal principal);
}
