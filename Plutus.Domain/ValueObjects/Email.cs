using FluentValidation;
using Plutus.Domain.Exceptions;
using ValueOf;

namespace Plutus.Domain.ValueObjects
{
    public class Email: ValueOf<string, Email>
    {
        private readonly EmailValidator _validator = new();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidEmailException"></exception>
        protected override void Validate()
        {
            if (_validator.Validate(this).IsValid is false)
                throw new InvalidEmailException();
        }
        
        /// <exception cref="InvalidEmailException"></exception>
        public static implicit operator Email(string email) => From(email.ToLower());
    }

    internal class EmailValidator : AbstractValidator<Email>
    {
        public EmailValidator()
        {
            RuleFor(e => e.Value).EmailAddress().NotEmpty();
        }
    }
}