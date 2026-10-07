using LocalVista.DTOs;
using LocalVista.Mapping;
using LocalVista.Repositories;

namespace LocalVista.Services;

public class CategoryService(ICategoryRepository categories) : ICategoryService
{
    public async Task<IReadOnlyList<CategoryDto>> ListAsync(CancellationToken ct)
    {
        var items = await categories.GetAllAsync(ct);
        return items.Select(AttractionMapper.ToDto).ToList();
    }
}
