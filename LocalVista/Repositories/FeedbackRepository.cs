using LocalVista.Data;
using LocalVista.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalVista.Repositories;

public class FeedbackRepository(LocalVistaDbContext db) : IFeedbackRepository
{
    public Task<bool> AttractionExistsAsync(int attractionId, CancellationToken ct) =>
        db.Attractions.AnyAsync(attraction => attraction.AttractionId == attractionId, ct);

    public async Task<IReadOnlyList<AttractionFeedback>> GetForAttractionAsync(
        int attractionId,
        CancellationToken ct) =>
        await db.AttractionFeedback
            .AsNoTracking()
            .Include(feedback => feedback.Attraction)
                .ThenInclude(attraction => attraction.Category)
            .Where(feedback => feedback.AttractionId == attractionId)
            .OrderByDescending(feedback => feedback.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<AttractionFeedback>> GetAllAsync(CancellationToken ct) =>
        await db.AttractionFeedback
            .AsNoTracking()
            .Include(feedback => feedback.Attraction)
                .ThenInclude(attraction => attraction.Category)
            .OrderByDescending(feedback => feedback.CreatedAtUtc)
            .ToListAsync(ct);

    public async Task<AttractionFeedback> AddAsync(AttractionFeedback feedback, CancellationToken ct)
    {
        db.AttractionFeedback.Add(feedback);
        await db.SaveChangesAsync(ct);
        return feedback;
    }
}
