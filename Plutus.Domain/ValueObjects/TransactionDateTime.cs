using System;
using FluentValidation;
using ValueOf;

namespace Plutus.Domain.ValueObjects
{
    public class TransactionDateTime: ValueOf<DateTime, TransactionDateTime>
    {
        public static readonly TransactionDateTimeValidator Validator = new();
        
        protected override void Validate()
        {
            if (Validator.Validate(this).IsValid is false)
                throw new ArgumentOutOfRangeException($"Transaction DateTime should be between Jan 1, 2000 and Dec 31, 2100");
        }
        
        public static implicit operator TransactionDateTime(DateTime dateTime) => From(dateTime);

        public static implicit operator DateTime(TransactionDateTime dateTime) => dateTime.Value;

        public static implicit operator TransactionDateTime(string dateTime) => DateTime.Parse(dateTime);
    }

    public class TransactionDateTimeValidator : AbstractValidator<DateTime>
    {
        public TransactionDateTimeValidator()
        {
            RuleFor(td => td)
                .GreaterThan(new DateTime(2000, 1, 1))
                .LessThan(new DateTime(2100, 1, 1));
        }
    }
}