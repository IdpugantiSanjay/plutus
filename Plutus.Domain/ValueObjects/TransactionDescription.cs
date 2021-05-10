using System;
using FluentValidation;
using ValueOf;


namespace Plutus.Domain.ValueObjects
{
    public class TransactionDescription: ValueOf<string, TransactionDescription>
    {
        public static readonly TransactionDescriptionValidator Validator = new();
        
        protected override void Validate()
        {
            var result = Validator.Validate(this);
            if (result.IsValid is false)
                throw new ArgumentOutOfRangeException($"Invalid Transaction Description. Description cannot be greater than 1024 characters");
        }

        public static implicit operator TransactionDescription(string description) => From(description);
        
        public static implicit operator string(TransactionDescription description) => description.Value;
        
        public class TransactionDescriptionValidator : AbstractValidator<string>
        {
            public TransactionDescriptionValidator()
            {
                RuleFor(td => td).MaximumLength(1024);
            }
        }
    }

    
}