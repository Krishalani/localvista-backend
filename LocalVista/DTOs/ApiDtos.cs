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
    decimal Longitude,
    IReadOnlyList<int> BestVisitMonths);

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
    decimal Longitude,
    IReadOnlyList<int>? BestVisitMonths = null);

public record LoginRequestDto(string Username, string Password);

public record AuthUserDto(string Username, string DisplayName, string Role);

public record LoginResponseDto(bool Ok, AuthUserDto? User = null, string? Error = null);

public record ApiErrorDto(string Message, IDictionary<string, string[]>? Errors = null);

public record FeedbackCreateDto(string? DisplayName, int Rating, string? Comment);

public record FeedbackDto(
    int Id,
    int AttractionId,
    string AttractionName,
    string Category,
    string DisplayName,
    int Rating,
    string Comment,
    DateTime CreatedAtUtc);

public record FeedbackSummaryDto(
    decimal AverageRating,
    int ReviewCount,
    IReadOnlyList<FeedbackDto> Reviews);

public record AttractionFeedbackSummaryDto(
    int AttractionId,
    string AttractionName,
    string Category,
    decimal AverageRating,
    int ReviewCount,
    IReadOnlyList<FeedbackDto> Reviews);
