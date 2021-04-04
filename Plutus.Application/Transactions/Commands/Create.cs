using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Plutus.Application.Repositories;
using Plutus.Domain;

namespace Plutus.Application.Transactions.Commands
{
    public static class CreateTransaction
    {
        public record Request(decimal Amount, DateTime DateTime, string Description, Guid CategoryId,
            string Username, TransactionType TransactionType,
            bool IsCredit = false) : IRequest<Response>;

        public record Response(Guid Id);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ITransactionRepository _transactionRepository;
            private readonly IMapper _mapper;

            public Handler(ITransactionRepository transactionRepository, IMapper mapper)
            {
                _transactionRepository = transactionRepository;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Request request,
                CancellationToken cancellationToken)
            {
                Transaction transaction = new(
                    Guid.NewGuid(),
                    TransactionType.Expense,
                    request.Username,
                    request.Amount,
                    request.DateTime,
                    request.CategoryId,
                    request.Description
                );

                var addedTransaction = await _transactionRepository.AddAsync(transaction);

                await _transactionRepository.SaveChangesAsync();

                return _mapper.Map<Response>(addedTransaction);
            }
        }
    }
}