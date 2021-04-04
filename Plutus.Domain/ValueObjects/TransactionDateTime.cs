using System;
using FluentValidation;
using ValueOf;

namespace Plutus.Domain.ValueObjects
{
    public class TransactionDateTime: ValueOf<DateTime, TransactionDateTime>
    {
        private readonly TransactionDateTimeValidator _validator = new();
        
        protected override void Validate()
        {
            if (_validator.Validate(this).IsValid is false)
                throw new ArgumentOutOfRangeException($"Transaction DateTime should be between Jan 1, 2000 and Dec 31, 2100");
        }
        
        public static implicit operator TransactionDateTime(DateTime dateTime) => From(dateTime);
    }

    internal class TransactionDateTimeValidator : AbstractValidator<TransactionDateTime>
    {
        public TransactionDateTimeValidator()
        {
            RuleFor(td => td.Value)
                .GreaterThan(new DateTime(2000, 1, 1))
                .LessThan(new DateTime(2100, 1, 1));
        }
    }
}