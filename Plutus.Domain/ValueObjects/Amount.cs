using System;
using FluentValidation;
using ValueOf;

namespace Plutus.Domain.ValueObjects
{
    public sealed class Amount: ValueOf<decimal, Amount>
    {
        public static readonly AmountValidator Validator = new();

        protected override void Validate()
        {
            var result = Validator.Validate(this);
            if (result.IsValid is false)
                throw new InvalidAmountException("Invalid Amount. Amount should be given between 0 and 100,000");
        }

        public static implicit operator Amount(decimal amount) => From(amount);
        
        public static implicit operator decimal(Amount amount) => amount.Value;
    }

    public class AmountValidator : AbstractValidator<decimal>
    {
        public AmountValidator()
        {
            RuleFor(a => a).NotEmpty().GreaterThan(0).LessThan(100_000);
        }
    }

    internal class InvalidAmountException : Exception
    {
        public InvalidAmountException(string message): base(message)
        {
            
        }
    }
}