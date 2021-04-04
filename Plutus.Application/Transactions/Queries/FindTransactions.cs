using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Plutus.Application.Repositories;
using Plutus.Domain;

namespace Plutus.Application.Transactions.Queries
{
    public static class FindTransactions
    {
        public record Request(string Username, DateTime From, DateTime To, Guid CategoryId,
            TransactionType TransactionType, string Description,
            int Skip = 0, int Limit = 100) : IRequest<Response>;

        public record Response(List<ResponseItem> List);

        public record ResponseItem(Guid Id, string Username, DateTime From, DateTime To, Category Category,
            TransactionType TransactionType, string Description);

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMapper _mapper;
            private readonly ITransactionRepository _repository;

            public Handler(IMapper mapper, ITransactionRepository repository)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var transactions = await _repository.FindAsync(request);
                return new Response(_mapper.Map<List<ResponseItem>>(transactions));
            }
        }
    }
}