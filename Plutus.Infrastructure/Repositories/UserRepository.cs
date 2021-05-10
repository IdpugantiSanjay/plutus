using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Plutus.Application.Repositories;
using Plutus.Domain;

namespace Plutus.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;


        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        // public async Task<bool> EmailExists(string email)
        // {
        //     var result = await _context.Users.SingleOrDefaultAsync(u => u.Email.Value == email);
        //     return result != default;
        // }


        public async Task<User> FindByUsername(string username)
        {
            var result = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            return result;
        }
        
        public async Task<User> FindByEmail(string email)
        {
            var result = await _context.Users.SingleOrDefaultAsync(u => u.Email.Value == email);
            return result;
        }

        // public async Task<bool> UsernameExists(string username)
        // {
        //     var result = await _context.Users.SingleOrDefaultAsync(u => u.Username.Value == username);
        //     return result != default;
        // }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}