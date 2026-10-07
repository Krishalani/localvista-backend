namespace LocalVista.DTOs;

public record AttractionDto(
    int Id,
    string Name,
    string Category,
    string Description,
    string OpeningHours,
    string TravelTips,
    decimal DistanceKm,
    IReadOnlyList<string> ImageUrls,
    decimal Latitude,
    decimal Longitude);

public record CategoryDto(int Id, string Name);

public record AttractionWriteDto(
    string Name,
    string Category,
    string Description,
    string? OpeningHours,
    string? TravelTips,
    decimal DistanceKm,
    IReadOnlyList<string> ImageUrls,
    decimal Latitude,
    decimal Longitude);

public record LoginRequestDto(string Username, string Password);

public record AuthUserDto(string Username, string DisplayName, string Role);

public record LoginResponseDto(bool Ok, AuthUserDto? User = null, string? Error = null);

public record ApiErrorDto(string Message, IDictionary<string, string[]>? Errors = null);
