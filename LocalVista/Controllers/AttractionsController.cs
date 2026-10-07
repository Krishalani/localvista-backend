using LocalVista.DTOs;
using LocalVista.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalVista.Controllers;

[ApiController]
[Route("api/attractions")]
public class AttractionsController(IAttractionService attractions) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<AttractionDto>>> List(
        [FromQuery] string? search,
        [FromQuery] string[]? categories,
        CancellationToken ct)
    {
        var categoryList = categories?
            .SelectMany(c => c.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Where(c => c.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var items = await attractions.ListAsync(search, categoryList, ct);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<AttractionDto>> GetById(int id, CancellationToken ct)
    {
        var item = await attractions.GetByIdAsync(id, ct);
        return item is null ? NotFound(new ApiErrorDto("Attraction was not found.")) : Ok(item);
    }

    [HttpPost]
    [Authorize(Roles = AuthService.AdminRole)]
    public async Task<ActionResult<AttractionDto>> Create(
        [FromBody] AttractionWriteDto request,
        CancellationToken ct)
    {
        var (dto, error) = await attractions.CreateAsync(request, ct);
        if (error is not null)
        {
            return BadRequest(new ApiErrorDto(error));
        }

        return CreatedAtAction(nameof(GetById), new { id = dto!.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AuthService.AdminRole)]
    public async Task<ActionResult<AttractionDto>> Update(
        int id,
        [FromBody] AttractionWriteDto request,
        CancellationToken ct)
    {
        var (dto, error) = await attractions.UpdateAsync(id, request, ct);
        if (error is null)
        {
            return Ok(dto);
        }

        if (error.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return NotFound(new ApiErrorDto(error));
        }

        return BadRequest(new ApiErrorDto(error));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AuthService.AdminRole)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var (ok, error) = await attractions.DeleteAsync(id, ct);
        if (ok)
        {
            return NoContent();
        }

        return NotFound(new ApiErrorDto(error ?? "Attraction was not found."));
    }
}
