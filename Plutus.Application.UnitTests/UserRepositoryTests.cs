using System.Threading.Tasks;
using Plutus.Domain;
using Plutus.Infrastructure.Repositories;
using Xunit;

namespace Plutus.Application.UnitTests
{
    public class UserRepositoryTests: PlutusTestBase
    {
        private readonly UserRepository _repository;
        
        private readonly User _userToAdd = new("isanjay", "sanjay112@outlook.com", "Sanjay", "Idpuganti", "Sanjay_11");

        public UserRepositoryTests()
        {
            var users = new[]
            {
                new User("sanjay", "sanjay11@outlook.com", "Sanjay", "Idpuganti", "Sanjay_11")
            };
            _context.Users.AddRange(users);
            _context.SaveChanges();
            _repository = new UserRepository(_context);
        }
        
        [Fact]
        public async Task ShouldReturnTrueWhenTakenUsernameIsGiven()
        {
            var exists = await _repository.UsernameAlreadyExists("sanjay");
            Assert.True(exists);
        }
        
        [Fact]
        public async Task ShouldReturnFalseWhenNewUsernameIsGiven()
        {
            var exists = await _repository.UsernameAlreadyExists("isanjay");
            Assert.False(exists);
        }
        
        [Fact]
        public async Task ShouldReturnTrueWhenExistingEmailIsGiven()
        {
            var exists = await _repository.EmailAlreadyExists("sanjay11@outlook.com");
            Assert.True(exists);
        }
        
        [Fact]
        public async Task ShouldReturnFalseWhenNonExistingEmailIsGiven()
        {
            var exists = await _repository.EmailAlreadyExists("isanjay@outlook.com");
            Assert.False(exists);
        }

        [Fact]
        public async Task AddShouldAddUser()
        {
            await _repository.AddAsync(_userToAdd);
            await _repository.SaveChangesAsync();
            Assert.True(await _repository.EmailAlreadyExists(_userToAdd.Email));
        }
    }
}