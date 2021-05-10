using System.Threading.Tasks;
using Plutus.Domain;
using Plutus.Infrastructure.Repositories;
using Xunit;

namespace Plutus.Application.UnitTests
{
    public class UserRepositoryTests : PlutusTestBase
    {
        private readonly UserRepository _repository;

        private readonly User _userToAdd = new("isanjay", "sanjay112@outlook.com", "sanjay_11", "Sanjay", "Idpuganti");

        public UserRepositoryTests()
        {
            var users = new[]
            {
                new User("sanjay", "sanjay11@outlook.com", "sanjay_11", "Sanjay", "Idpuganti")
            };
            _context.Users.AddRange(users);
            _context.SaveChanges();
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task ShouldReturnTrueWhenTakenUsernameIsGiven()
        {
            var exists = await _repository.FindByUsername("sanjay");
            Assert.True(exists is not null);
        }

        [Fact]
        public async Task ShouldReturnFalseWhenNewUsernameIsGiven()
        {
            var exists = await _repository.FindByUsername("isanjay");
            Assert.False(exists is not null);
        }

        [Fact]
        public async Task ShouldReturnTrueWhenExistingEmailIsGiven()
        {
            var exists = await _repository.FindByEmail("sanjay11@outlook.com");
            Assert.True(exists is not null);
        }

        [Fact]
        public async Task ShouldReturnFalseWhenNonExistingEmailIsGiven()
        {
            var exists = await _repository.FindByEmail("isanjay@outlook.com");
            Assert.False(exists is not null);
        }

        [Fact]
        public async Task AddShouldAddUser()
        {
            await _repository.AddAsync(_userToAdd);
            await _repository.SaveChangesAsync();
            Assert.True(await _repository.FindByEmail(_userToAdd.Email) is not null);
        }
    }
}