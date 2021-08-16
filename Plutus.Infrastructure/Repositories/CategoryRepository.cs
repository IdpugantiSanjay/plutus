using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Plutus.Application.Repositories;
using Plutus.Domain;

namespace Plutus.Infrastructure.Repositories;


public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Category>> FindAsync() => _context.Categories.ToListAsync();

    private Dictionary<Guid, Category> categoryCache = new(10);

    public async Task<Category?> FindByIdAsync(Guid id)
    {
        if (categoryCache.ContainsKey(id)) return categoryCache[id];
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category is not null) categoryCache.TryAdd(id, category);
        return category;
    }
}
