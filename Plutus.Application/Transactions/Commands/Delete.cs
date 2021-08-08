using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Indexes;
using Plutus.Domain.ValueObjects;

namespace Plutus.Application.Transactions.Commands
{
    public static class DeleteTransaction
    {
        public record Request(Guid Id) : IRequest<Response>;

        public record Response(Guid Id);


        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ITransactionRepository _repository;
            private readonly TransactionIndex _transactionIndex;

            public Handler(ITransactionRepository repository, TransactionIndex transactionIndex)
            {
                _repository = repository;
                _transactionIndex = transactionIndex;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var transactionToDelete = await _repository.FindByIdAsync(request.Id);
                transactionToDelete.MakeInActive();
                _repository.Update(transactionToDelete);
                await _repository.SaveChangesAsync();
                
                await _transactionIndex.DeleteAsync(request.Id.ToString(), cancellationToken);
                return new Response(request.Id);
            }
        }
        
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.Id).NotEmpty();
            }
        }
    }
}