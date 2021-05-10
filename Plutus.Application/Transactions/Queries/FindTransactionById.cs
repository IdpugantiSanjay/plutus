using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.ViewModels.cs;

namespace Plutus.Application.Transactions.Queries
{
    public static class FindTransactionById
    {
        public record Request(Guid Id): IRequest<TransactionViewModel>;
        
        public class Handler: IRequestHandler<Request, TransactionViewModel>
        {
            private readonly IMapper _mapper;
            private readonly ITransactionRepository _repository;

            public Handler(IMapper mapper, ITransactionRepository repository)
            {
                _mapper = mapper;
                _repository = repository;
            }
            
            public async Task<TransactionViewModel> Handle(Request request, CancellationToken cancellationToken)
            {
                var foundTransaction = await _repository.FindByIdAsync(request.Id);
                var vm = _mapper.Map<TransactionViewModel>(foundTransaction);
                return vm;
            }
        }
    }
}