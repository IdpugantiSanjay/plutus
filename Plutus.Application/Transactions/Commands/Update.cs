using System;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Plutus.Application.Repositories;
using Plutus.Domain;

namespace Plutus.Application.Transactions.Commands
{
    public static class UpdateTransaction
    {
        public record Request(Guid TransactionId, decimal Amount, DateTime DateTime, string Description,
            Guid CategoryId,
            string Username, TransactionType TransactionType, bool IsCredit = false) : IRequest<Response>;

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
            
            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                Transaction updatedTransaction = new(
                    request.TransactionId,
                    request.TransactionType,
                    request.Username,
                    request.Amount,
                    request.DateTime,
                    request.CategoryId
                );

                var transactionInDb = await _transactionRepository.FindByIdAsync(request.TransactionId);
                if (transactionInDb is null)
                    throw new ArgumentException($"Invalid Transaction Id: {request.TransactionId}");
                
                transactionInDb.Update(updatedTransaction);
                _transactionRepository.Update(transactionInDb);
                await _transactionRepository.SaveChangesAsync();

                return _mapper.Map<Response>(transactionInDb);
            }
        }
    }
}