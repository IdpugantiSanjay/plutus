using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plutus.Domain;

namespace Plutus.Application.Repositories
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> FindAsync();

        public Task<Category> FindByIdAsync(Guid id);
    }
}