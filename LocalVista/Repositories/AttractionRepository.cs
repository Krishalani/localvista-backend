using LocalVista.Data;
using LocalVista.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalVista.Repositories;

public class AttractionRepository(LocalVistaDbContext db) : IAttractionRepository
{
    public async Task<IReadOnlyList<Attraction>> QueryAsync(
        string? search,
        IReadOnlyList<string>? categories,
        double? maxDistanceKm,
        CancellationToken ct)
    {
        var query = db.Attractions
            .AsNoTracking()
            .Include(a => a.Category)
            .Include(a => a.Images)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(a =>
                a.Name.Contains(term) ||
                a.Description.Contains(term) ||
                (a.TravelTips != null && a.TravelTips.Contains(term)) ||
                (a.OpeningHours != null && a.OpeningHours.Contains(term)) ||
                a.Category.Name.Contains(term));
        }

        if (categories is { Count: > 0 })
        {
            query = query.Where(a => categories.Contains(a.Category.Name));
        }

        if (maxDistanceKm is >= 0)
        {
            var maximumDistance = (decimal)maxDistanceKm.Value;
            query = query.Where(a => a.DistanceKm <= maximumDistance);
        }

        return await query
            .OrderBy(a => a.Name)
            .ToListAsync(ct);
    }

    public Task<Attraction?> GetByIdAsync(int id, CancellationToken ct) =>
        db.Attractions
            .Include(a => a.Category)
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.AttractionId == id, ct);

    public async Task<Attraction> AddAsync(Attraction attraction, CancellationToken ct)
    {
        db.Attractions.Add(attraction);
        await db.SaveChangesAsync(ct);
        return attraction;
    }

    public async Task UpdateAsync(Attraction attraction, CancellationToken ct)
    {
        attraction.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Attraction attraction, CancellationToken ct)
    {
        db.Attractions.Remove(attraction);
        await db.SaveChangesAsync(ct);
    }

    public async Task ReplaceImagesAsync(
        Attraction attraction,
        IReadOnlyList<string> imageUrls,
        CancellationToken ct)
    {
        db.AttractionImages.RemoveRange(attraction.Images);
        attraction.Images.Clear();

        var order = 0;
        foreach (var url in imageUrls.Select(u => u.Trim()).Where(u => u.Length > 0))
        {
            attraction.Images.Add(new AttractionImage
            {
                ImageUrl = url,
                SortOrder = order++,
            });
        }

        await db.SaveChangesAsync(ct);
    }
}
