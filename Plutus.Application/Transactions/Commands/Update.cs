using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plutus.Application.Exceptions;
using Plutus.Application.Repositories;
using Plutus.Domain;
using Plutus.Domain.Enums;
using Plutus.Domain.ValueObjects;

namespace Plutus.Application.Transactions.Commands
{
    public static class UpdateTransaction
    {
        public record Request(Guid Id, decimal Amount, DateTime DateTime,
            string Description, Guid CategoryId, [FromRoute] string Username, TransactionType TransactionType) : IRequest<(AbstractPlutusException?, Response?)>;

        public record Response(Guid Id);

        public class Handler : IRequestHandler<Request, (AbstractPlutusException?, Response?)>
        {
            private readonly ITransactionRepository _transactionRepository;
            private readonly IMapper _mapper;

            public Handler(ITransactionRepository transactionRepository, IMapper mapper)
            {
                _transactionRepository = transactionRepository;
                _mapper = mapper;
            }

            public async Task<(AbstractPlutusException?, Response?)> Handle(Request request, CancellationToken cancellationToken)
            {
                Transaction updatedTransaction = new(
                    request.Id,
                    request.TransactionType,
                    request.Username,
                    request.Amount,
                    request.DateTime,
                    request.CategoryId,
                    request.Description
                );

                var transactionInDb = await _transactionRepository.FindByIdAsync(request.Id);

                if (transactionInDb is null)
                    return (new TransactionNotFoundException(), null);

                transactionInDb.Update(updatedTransaction);
                _transactionRepository.Update(transactionInDb);
                await _transactionRepository.SaveChangesAsync();

                return (null, _mapper.Map<Response>(transactionInDb));
            }
        }
        
        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.Id).NotEmpty();
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