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

    public Task<Category> FindByIdAsync(Guid id) => _context.Categories.FirstAsync(c => c.Id == id);
}
