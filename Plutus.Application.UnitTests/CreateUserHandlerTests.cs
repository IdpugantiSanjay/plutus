using System.Threading;
using System.Threading.Tasks;
using Plutus.Application.Repositories;
using Plutus.Application.Users.Commands;
using Xunit;
using Plutus.Domain;
using Plutus.Infrastructure.Repositories;

namespace Plutus.Application.UnitTests
{
    public class CreateUserHandlerTests: PlutusTestBase
    {
        private readonly Create.Handler _handler;

        public CreateUserHandlerTests()
        {
            var users = new[]
            {
                new User("sanjay", "sanjay_11","sanjay11@outlook.com", "Sanjay", "Idpuganti")
            };
            _context.Users.AddRange(users);
            _context.SaveChanges();
            var repository = new UserRepository(_context);
            _handler = new Create.Handler(repository);
        }

        [Fact]
        public async Task ShouldReturnCreatedUsername()
        {
            var request = new Create.Request("isanjay", "sanjay_11","Sanjay","Idpuganti", "sanjay112@outlook.com");
            var handledResult = await _handler.Handle(request,
                CancellationToken.None);
           Assert.Equal(handledResult.Username, request.Username); 
        }
    }
}