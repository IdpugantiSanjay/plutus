using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Plutus.Application.Users.Commands;
using Xunit;
using Xunit.Abstractions;

namespace Plutus.Api.IntegrationTests
{
    public class UserControllerTests: IClassFixture<IntegrationTestsApplicationFactory>
    {
        private readonly IntegrationTestsApplicationFactory _factory;
        private readonly ITestOutputHelper _output;

        public UserControllerTests(IntegrationTestsApplicationFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public async Task ShouldReturnTokenWhenValidUsernamePasswordAreGiven()
        {
            var client = _factory.CreateClient();
            Authenticate.Request request = new("sanjay", "Sanjay_11!");
            
            var httpResponse = await client.PostAsync("api/users/authenticate", JsonContent.Create(request));

            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<Authenticate.Response>();
            
            Assert.NotNull(response);
            Assert.NotNull(response!.Token);
            Assert.True(response!.Token.Length > 10);
        }
    }
}