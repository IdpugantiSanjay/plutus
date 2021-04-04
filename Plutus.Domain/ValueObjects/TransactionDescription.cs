using System;
using FluentValidation;
using ValueOf;


namespace Plutus.Domain.ValueObjects
{
    public class TransactionDescription: ValueOf<string, TransactionDescription>
    {
        private readonly TransactionDescriptionValidator _validator = new();
        
        protected override void Validate()
        {
            var result = _validator.Validate(this);
            if (result.IsValid is false)
                throw new ArgumentOutOfRangeException($"Invalid Transaction Description. Description cannot be greater than 1024 characters");
        }

        public static implicit operator TransactionDescription(string description) => TransactionDescription.From(description);
    }

    internal class TransactionDescriptionValidator : AbstractValidator<TransactionDescription>
    {
        public TransactionDescriptionValidator()
        {
            RuleFor(td => td.Value).MaximumLength(1024);
        }
    }
}