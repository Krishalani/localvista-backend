using LocalVista.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalVista.Repositories;

public interface IAttractionRepository
{
    Task<IReadOnlyList<Attraction>> QueryAsync(string? search, IReadOnlyList<string>? categories, CancellationToken ct);
    Task<Attraction?> GetByIdAsync(int id, CancellationToken ct);
    Task<Attraction> AddAsync(Attraction attraction, CancellationToken ct);
    Task UpdateAsync(Attraction attraction, CancellationToken ct);
    Task DeleteAsync(Attraction attraction, CancellationToken ct);
    Task ReplaceImagesAsync(Attraction attraction, IReadOnlyList<string> imageUrls, CancellationToken ct);
}

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct);
    Task<Category?> FindByNameAsync(string name, CancellationToken ct);
}
