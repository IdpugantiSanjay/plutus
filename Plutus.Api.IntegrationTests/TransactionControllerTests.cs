using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Commands;
using Plutus.Domain.Enums;
using Plutus.Infrastructure;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Plutus.Api.IntegrationTests
{
    public class TransactionControllerTests : IClassFixture<IntegrationTestsApplicationFactory>
    {
        private readonly IntegrationTestsApplicationFactory _factory;
        private readonly ITestOutputHelper _output;
        private readonly AppDbContext _context;

        private readonly Guid _foodAndDrinksId;

        public TransactionControllerTests(IntegrationTestsApplicationFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;

            _context = (factory.Services.GetService(typeof(AppDbContext)) as AppDbContext)!;

            _foodAndDrinksId = _context.Categories.First(c => c.Name == "Food & Drinks").Id;

            Log.Information("Food and Drinks Id: " + _foodAndDrinksId);
        }

        [Fact]
        public async Task ShouldReturnTransactionIdWhenCorrectTransactionIsProvided()
        {
            var client = _factory.CreateClient();

            CreateTransaction.Request request =
                new(10, DateTime.Now, "Test", _foodAndDrinksId, "sanjay", TransactionType.Expense);

            var httpResponse = await client.PostAsync("api/users/transactions", JsonContent.Create(request));
            httpResponse.EnsureSuccessStatusCode();

            Assert.True(httpResponse.IsSuccessStatusCode);
            var response = await httpResponse.Content.ReadFromJsonAsync<CreateTransaction.Response>();
            Assert.True(response!.Id != default);
        }

        [Fact]
        public async Task ShouldThrowAnErrorWhenReferencialIntegrityBreaksForUsername()
        {
            var client = _factory.CreateClient();
            
            CreateTransaction.Request request =
                new(10, DateTime.Now, "Test", _foodAndDrinksId, "randomusrnme", TransactionType.Expense);

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
                await client.PostAsync("api/users/transactions", JsonContent.Create(request)));
        }
        
        [Fact]
        public async Task ShouldThrowAnErrorWhenReferencialIntegrityBreaksForCategory()
        {
            var client = _factory.CreateClient();
            
            CreateTransaction.Request request =
                new(10, DateTime.Now, "Test", Guid.NewGuid(), "sanjay", TransactionType.Expense);
            
            await Assert.ThrowsAsync<DbUpdateException>(async () => await client.PostAsync("api/users/transactions", JsonContent.Create(request)));
        }
    }
}