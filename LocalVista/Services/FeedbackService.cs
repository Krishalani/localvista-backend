using LocalVista.Data.Entities;
using LocalVista.DTOs;
using LocalVista.Repositories;

namespace LocalVista.Services;

public class FeedbackService(IFeedbackRepository feedback) : IFeedbackService
{
    public async Task<FeedbackSummaryDto?> GetForAttractionAsync(int attractionId, CancellationToken ct)
    {
        if (!await feedback.AttractionExistsAsync(attractionId, ct))
        {
            return null;
        }

        var rows = await feedback.GetForAttractionAsync(attractionId, ct);
        var average = rows.Count == 0 ? 0 : decimal.Round(rows.Average(row => (decimal)row.Rating), 1);
        return new FeedbackSummaryDto(average, rows.Count, rows.Take(30).Select(ToDto).ToArray());
    }

    public async Task<(FeedbackDto? Dto, string? Error, bool NotFound)> CreateAsync(
        int attractionId,
        FeedbackCreateDto request,
        CancellationToken ct)
    {
        var name = request.DisplayName?.Trim();
        var comment = request.Comment?.Trim();

        if (request.Rating is < 1 or > 5)
        {
            return (null, "Choose a rating from 1 to 5 stars.", false);
        }

        if (name?.Length > 80)
        {
            return (null, "Name must be 80 characters or fewer.", false);
        }

        if (comment?.Length > 1000)
        {
            return (null, "Feedback must be 1000 characters or fewer.", false);
        }

        if (!await feedback.AttractionExistsAsync(attractionId, ct))
        {
            return (null, "Attraction was not found.", true);
        }

        var saved = await feedback.AddAsync(new AttractionFeedback
        {
            AttractionId = attractionId,
            DisplayName = string.IsNullOrWhiteSpace(name) ? null : name,
            Rating = request.Rating,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment,
            CreatedAtUtc = DateTime.UtcNow,
        }, ct);

        var summary = await feedback.GetForAttractionAsync(attractionId, ct);
        var created = summary.First(row => row.AttractionFeedbackId == saved.AttractionFeedbackId);
        return (ToDto(created), null, false);
    }

    public async Task<IReadOnlyList<AttractionFeedbackSummaryDto>> GetAllForAdminAsync(CancellationToken ct)
    {
        var rows = await feedback.GetAllAsync(ct);
        return rows
            .GroupBy(row => row.AttractionId)
            .Select(group =>
            {
                var first = group.First();
                return new AttractionFeedbackSummaryDto(
                    first.AttractionId,
                    first.Attraction.Name,
                    first.Attraction.Category.Name,
                    decimal.Round(group.Average(row => (decimal)row.Rating), 1),
                    group.Count(),
                    group.Select(ToDto).ToArray());
            })
            .OrderByDescending(item => item.ReviewCount)
            .ThenBy(item => item.AttractionName)
            .ToArray();
    }

    private static FeedbackDto ToDto(AttractionFeedback row) => new(
        row.AttractionFeedbackId,
        row.AttractionId,
        row.Attraction.Name,
        row.Attraction.Category.Name,
        string.IsNullOrWhiteSpace(row.DisplayName) ? "Local visitor" : row.DisplayName,
        row.Rating,
        row.Comment ?? string.Empty,
        row.CreatedAtUtc);
}
