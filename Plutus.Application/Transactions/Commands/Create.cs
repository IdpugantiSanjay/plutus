using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Plutus.Application.Repositories;
using Plutus.Domain;
using Plutus.Domain.Enums;
using Plutus.Domain.ValueObjects;


namespace Plutus.Application.Transactions.Commands
{
    public static class CreateTransaction
    {
        public record Request(decimal Amount, DateTime DateTime, string Description, Guid CategoryId,
            string Username, TransactionType TransactionType) : IRequest<Response>;

        public class Response
        {
            public Guid Id { get; init; }
        }

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
                var response =_mapper.Map<Response>(addedTransaction);
                return response;
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
                // RuleFor(r => r.TransactionType).NotEmpty();
            }
        }
    }
}