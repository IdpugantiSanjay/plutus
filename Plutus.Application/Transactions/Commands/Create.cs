using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Indexes;
using Plutus.Domain;
using Plutus.Domain.Enums;
using Plutus.Domain.ValueObjects;

namespace Plutus.Application.Transactions.Commands;
public static class CreateTransaction
{
    public record Request(decimal Amount, DateTime DateTime, string Description, Guid CategoryId,
        [FromRoute] string Username, TransactionType TransactionType) : IRequest<Response>;

    public class Response
    {
        public Guid Id { get; init; }
    }

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;
        private readonly TransactionIndex _trxIndex;

        public Handler(ITransactionRepository repository, IMapper mapper, TransactionIndex trxIndex)
        {
            _repository = repository;
            _mapper = mapper;
            _trxIndex = trxIndex;
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


            var addedTransaction = await _repository.AddAsync(transaction);

            if (addedTransaction != null)
            {
                var response = _mapper.Map<Response>(addedTransaction);

                await _trxIndex.IndexAsync(new TransactionIndex.Transaction(addedTransaction.Id, addedTransaction.Username,
                    addedTransaction.Category.Name, addedTransaction.DateTime, addedTransaction.Amount,
                    addedTransaction?.Description ?? "", addedTransaction!.TransactionType == TransactionType.Income), cancellationToken);

                return response;
            }

            throw new InvalidOperationException();
        }
    }


    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Amount).SetValidator(a => Amount.Validator);
            RuleFor(r => r.DateTime).SetValidator(a => TransactionDateTime.Validator);
            RuleFor(r => r.Description).SetValidator(a => TransactionDescription.Validator);
            RuleFor(r => r.Username).SetValidator(a => Username.Validator);
            RuleFor(r => r.CategoryId).NotEmpty();
        }
    }
}
