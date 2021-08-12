using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Plutus.Application.Exceptions;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.ViewModels.cs;

namespace Plutus.Application.Transactions.Queries;

public static class FindTransactionById
{
    public record Request(Guid Id) : IRequest<(AbstractPlutusException?, TransactionViewModel?)>;

    public class Handler : IRequestHandler<Request, (AbstractPlutusException?, TransactionViewModel?)>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _repository;

        public Handler(IMapper mapper, ITransactionRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<(AbstractPlutusException?, TransactionViewModel?)> Handle(Request request, CancellationToken cancellationToken)
        {
            var foundTransaction = await _repository.FindByIdAsync(request.Id);
            if (foundTransaction == null) return (new TransactionNotFoundException(), null);
            var vm = _mapper.Map<TransactionViewModel>(foundTransaction);
            return (null, vm);
        }
    }
}
