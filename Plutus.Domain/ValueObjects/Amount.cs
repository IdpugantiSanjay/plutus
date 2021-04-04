using System;
using FluentValidation;
using ValueOf;

namespace Plutus.Domain.ValueObjects
{
    public sealed class Amount: ValueOf<decimal, Amount>
    {
        private readonly AmountValidator _validator = new();

        protected override void Validate()
        {
            var result = _validator.Validate(this);
            if (result.IsValid is false)
                throw new InvalidAmountException("Invalid Amount. Amount should be given between 0 and 100,000");
        }

        public static implicit operator Amount(decimal amount) => From(amount);
    }

    internal class AmountValidator : AbstractValidator<Amount>
    {
        public AmountValidator()
        {
            RuleFor(a => a.Value).NotEmpty().GreaterThan(0).LessThan(100_000);
        }
    }

    internal class InvalidAmountException : Exception
    {
        public InvalidAmountException(string message): base(message)
        {
            
        }
    }
}