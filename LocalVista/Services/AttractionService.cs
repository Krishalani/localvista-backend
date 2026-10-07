using LocalVista.Data.Entities;
using LocalVista.DTOs;
using LocalVista.Mapping;
using LocalVista.Repositories;

namespace LocalVista.Services;

public class AttractionService(
    IAttractionRepository attractions,
    ICategoryRepository categories) : IAttractionService
{
    public async Task<IReadOnlyList<AttractionDto>> ListAsync(
        string? search,
        IReadOnlyList<string>? categoriesFilter,
        CancellationToken ct)
    {
        var items = await attractions.QueryAsync(search, categoriesFilter, ct);
        return items.Select(AttractionMapper.ToDto).ToList();
    }

    public async Task<AttractionDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var item = await attractions.GetByIdAsync(id, ct);
        return item is null ? null : AttractionMapper.ToDto(item);
    }

    public async Task<(AttractionDto? Dto, string? Error)> CreateAsync(
        AttractionWriteDto request,
        CancellationToken ct)
    {
        var validation = Validate(request);
        if (validation is not null)
        {
            return (null, validation);
        }

        var category = await categories.FindByNameAsync(request.Category.Trim(), ct);
        if (category is null)
        {
            return (null, "Category was not found.");
        }

        if (request.DistanceKm < 0 || request.DistanceKm > 25)
        {
            return (null, "Distance must be between 0 and 25 km from Kandy.");
        }

        var now = DateTime.UtcNow;
        var imageUrls = NormalizeImages(request.ImageUrls);
        var entity = new Attraction
        {
            CategoryId = category.CategoryId,
            Category = category,
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            OpeningHours = request.OpeningHours?.Trim(),
            TravelTips = request.TravelTips?.Trim(),
            DistanceKm = request.DistanceKm,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            Images = imageUrls
                .Select((url, index) => new AttractionImage
                {
                    ImageUrl = url,
                    SortOrder = index,
                })
                .ToList(),
        };

        await attractions.AddAsync(entity, ct);
        var created = await attractions.GetByIdAsync(entity.AttractionId, ct);
        return (AttractionMapper.ToDto(created!), null);
    }

    public async Task<(AttractionDto? Dto, string? Error)> UpdateAsync(
        int id,
        AttractionWriteDto request,
        CancellationToken ct)
    {
        var validation = Validate(request);
        if (validation is not null)
        {
            return (null, validation);
        }

        var entity = await attractions.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return (null, "Attraction was not found.");
        }

        var category = await categories.FindByNameAsync(request.Category.Trim(), ct);
        if (category is null)
        {
            return (null, "Category was not found.");
        }

        if (request.DistanceKm < 0 || request.DistanceKm > 25)
        {
            return (null, "Distance must be between 0 and 25 km from Kandy.");
        }

        entity.CategoryId = category.CategoryId;
        entity.Category = category;
        entity.Name = request.Name.Trim();
        entity.Description = request.Description.Trim();
        entity.OpeningHours = request.OpeningHours?.Trim();
        entity.TravelTips = request.TravelTips?.Trim();
        entity.DistanceKm = request.DistanceKm;
        entity.Latitude = request.Latitude;
        entity.Longitude = request.Longitude;

        await attractions.ReplaceImagesAsync(entity, NormalizeImages(request.ImageUrls), ct);
        await attractions.UpdateAsync(entity, ct);

        var updated = await attractions.GetByIdAsync(id, ct);
        return (AttractionMapper.ToDto(updated!), null);
    }

    public async Task<(bool Ok, string? Error)> DeleteAsync(int id, CancellationToken ct)
    {
        var entity = await attractions.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return (false, "Attraction was not found.");
        }

        await attractions.DeleteAsync(entity, ct);
        return (true, null);
    }

    private static string? Validate(AttractionWriteDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Category) ||
            string.IsNullOrWhiteSpace(request.Description))
        {
            return "Name, category, and description are required.";
        }

        if (NormalizeImages(request.ImageUrls).Count == 0)
        {
            return "At least one image URL is required.";
        }

        return null;
    }

    private static List<string> NormalizeImages(IReadOnlyList<string>? urls) =>
        (urls ?? Array.Empty<string>())
            .Select(u => u.Trim())
            .Where(u => u.Length > 0)
            .ToList();
}
