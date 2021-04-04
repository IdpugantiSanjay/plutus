using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Plutus.Domain;

namespace Plutus.Application.Repositories
{
    public interface IUserRepository
    {
        public Task AddAsync(User user);

        
        public Task<bool> EmailAlreadyExists(string email);
        
        public Task<bool> UsernameAlreadyExists(string username);

        public Task SaveChangesAsync();
    }
}