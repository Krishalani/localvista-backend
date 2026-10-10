using LocalVista.DTOs;
using LocalVista.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalVista.Controllers;

[ApiController]
[Route("api/attractions/{attractionId:int}/feedback")]
public class FeedbackController(IFeedbackService feedback) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<FeedbackSummaryDto>> Get(int attractionId, CancellationToken ct)
    {
        var summary = await feedback.GetForAttractionAsync(attractionId, ct);
        return summary is null
            ? NotFound(new ApiErrorDto("Attraction was not found."))
            : Ok(summary);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<FeedbackDto>> Create(
        int attractionId,
        [FromBody] FeedbackCreateDto request,
        CancellationToken ct)
    {
        var (dto, error, notFound) = await feedback.CreateAsync(attractionId, request, ct);
        if (notFound)
        {
            return NotFound(new ApiErrorDto(error!));
        }

        if (error is not null)
        {
            return BadRequest(new ApiErrorDto(error));
        }

        return CreatedAtAction(nameof(Get), new { attractionId }, dto);
    }
}

[ApiController]
[Route("api/admin/feedback")]
[Authorize(Roles = AuthService.AdminRole)]
public class AdminFeedbackController(IFeedbackService feedback) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AttractionFeedbackSummaryDto>>> GetAll(CancellationToken ct) =>
        Ok(await feedback.GetAllForAdminAsync(ct));
}
