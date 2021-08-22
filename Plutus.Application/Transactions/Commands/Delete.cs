using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Exceptions;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Indexes;
using Plutus.Domain.ValueObjects;

namespace Plutus.Application.Transactions.Commands
{
    public static class DeleteTransaction
    {
        public record Request(Guid Id) : IRequest<(AbstractPlutusException? error, Response? response)>;

        public record Response(Guid Id);


        public class Handler : IRequestHandler<Request, (AbstractPlutusException? error, Response? response)>
        {
            private readonly ITransactionRepository _repository;
            private readonly TransactionIndex _transactionIndex;

            public Handler(ITransactionRepository repository, TransactionIndex transactionIndex)
            {
                _repository = repository;
                _transactionIndex = transactionIndex;
            }

            public async Task<(AbstractPlutusException? error, Response? response)> Handle(Request request, CancellationToken cancellationToken)
            {
                var transactionToDelete = await _repository.FindByIdAsync(request.Id);

                if (transactionToDelete is null || transactionToDelete.InActive) return (new TransactionNotFoundException(), null);

                transactionToDelete.MakeInActive();
                _repository.Update(transactionToDelete);
                await _repository.SaveChangesAsync();
                
                await _transactionIndex.DeleteAsync(request.Id.ToString(), cancellationToken);
                return (null, new Response(request.Id));
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