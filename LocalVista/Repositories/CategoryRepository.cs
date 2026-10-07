using LocalVista.Data;
using LocalVista.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalVista.Repositories;

public class CategoryRepository(LocalVistaDbContext db) : ICategoryRepository
{
    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct) =>
        await db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public Task<Category?> FindByNameAsync(string name, CancellationToken ct) =>
        db.Categories.FirstOrDefaultAsync(c => c.Name == name, ct);
}
