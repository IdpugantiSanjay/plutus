using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Plutus.Application.Repositories;

namespace Plutus.Application.Transactions.Commands
{
    public class Delete
    {
        public record Request(Guid Id) : IRequest<Response>;

        public record Response(Guid Id);


        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ITransactionRepository _repository;

            public Handler(ITransactionRepository repository)
            {
                _repository = repository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var transactionToDelete = await _repository.FindByIdAsync(request.Id);
                transactionToDelete.MakeInActive();
                _repository.Update(transactionToDelete);
                await _repository.SaveChangesAsync();
                return new Response(request.Id);
            }
        }
    }
}