using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Indexes;
using Plutus.Application.Transactions.ViewModels.cs;
using Plutus.Domain;
using Plutus.Domain.Enums;
using Plutus.Domain.ValueObjects;

namespace Plutus.Application.Transactions.Queries;

public static class FindTransactions
{
    public record Request([FromRoute] string Username, DateTime From, DateTime To, Guid CategoryId, string? Description, int Skip = 0, int Limit = 100) : IRequest<IEnumerable<TransactionViewModel>>;

    public class Handler : IRequestHandler<Request, IEnumerable<TransactionViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _repository;
        private readonly TransactionIndex _trxIndex;

        public Handler(IMapper mapper, ITransactionRepository repository, TransactionIndex trxIndex)
        {
            _mapper = mapper;
            _repository = repository;
            _trxIndex = trxIndex;
        }

        public async Task<IEnumerable<TransactionViewModel>> Handle(Request request, CancellationToken cancellationToken)
        {
            var transactions = await _trxIndex.FindAsync(request);
            var vm = _mapper.Map<IEnumerable<TransactionViewModel>>(transactions);
            return vm;
        }
    }
}
