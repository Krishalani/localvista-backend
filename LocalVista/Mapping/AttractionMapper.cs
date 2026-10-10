using LocalVista.Data.Entities;
using LocalVista.DTOs;

namespace LocalVista.Mapping;

public static class AttractionMapper
{
    public static AttractionDto ToDto(Attraction attraction) =>
        new(
            attraction.AttractionId,
            attraction.Name,
            attraction.Category.Name,
            attraction.Description,
            attraction.OpeningHours ?? string.Empty,
            attraction.TravelTips ?? string.Empty,
            attraction.DistanceKm,
            attraction.Images
                .OrderBy(i => i.SortOrder)
                .Select(i => i.ImageUrl)
                .ToList(),
            attraction.Latitude,
            attraction.Longitude,
            attraction.BestVisitMonths
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(value => int.TryParse(value, out var month) ? month : 0)
                .Where(month => month is >= 1 and <= 12)
                .ToArray());

    public static CategoryDto ToDto(Category category) =>
        new(category.CategoryId, category.Name);
}
