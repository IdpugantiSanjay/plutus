using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Plutus.Domain;

namespace Plutus.Application.Repositories
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);

        public Task<User> FindByEmail(string email);
        
        public Task<User> FindByUsername(string username);

        public Task SaveChangesAsync();
    }
}