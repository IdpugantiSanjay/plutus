using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Plutus.Application.MappingProfiles;
using Plutus.Application.Transactions.Commands;
using Plutus.Domain;
using Plutus.Domain.Enums;
using Plutus.Infrastructure.Repositories;
using Shouldly;
using Xunit;
using Profile = Plutus.Application.MappingProfiles.Profile;

namespace Plutus.Application.UnitTests
{
    public class CreateTransactionHandler : PlutusTestBase
    {
        private CreateTransaction.Handler _handler;

        private readonly Guid _travelCategoryId;

        public CreateTransactionHandler()
        {
            var users = new[]
            {
                new User("sanjay", "sanjay_11", "sanjay11@outlook.com", "Sanjay", "Idpuganti")
            };
            _context.Users.AddRange(users);

            _travelCategoryId = _context.Categories.Single(c => c.Name == "Travel").Id;

            _context.SaveChanges();

            var transactionRepository = new TransactionRepository(_context);
            _handler = new CreateTransaction.Handler(transactionRepository,
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new Profile()))));
        }

        [Fact]
        public async Task ShouldCreateTransactionIfValid()
        {
            var request =
                new CreateTransaction.Request(10, DateTime.Now, "", _travelCategoryId, "sanjay",
                    TransactionType.Expense);
            var response = await _handler.Handle(request, CancellationToken.None);
            response.Id.ShouldNotBe(default);
        }

        // Foreign Key Tests

        // [Fact]
        // public async Task ShouldThrowExceptionIfCategoryDoesNotExistInDatabase()
        // {
        //     var request =
        //         new CreateTransaction.Request(10, DateTime.Now, "", Guid.NewGuid(), "sanjay", TransactionType.Expense);
        //     await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(request, CancellationToken.None));
        // }
        //
        // [Fact]
        // public async Task ShouldThrowExceptionIfUserDoesNotExistInDatabase()
        // {
        //     const string validUsername = "sanjay11";
        //     
        //     var request =
        //         new CreateTransaction.Request(10, DateTime.Now, "", _travelCategoryId, validUsername, TransactionType.Expense);
        //     await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(request, CancellationToken.None));
        // }
    }
}