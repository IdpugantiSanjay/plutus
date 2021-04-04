using System.Linq;
using FluentValidation;
using Plutus.Domain.Exceptions;
using ValueOf;

namespace Plutus.Domain.ValueObjects
{
    public class Password : ValueOf<string, Password>
    {
        private readonly PasswordValidator _validator = new();


        protected override void Validate()
        {
            if (_validator.Validate(this).IsValid is false)
                throw new InvalidPasswordException();
        }
        
        public static implicit operator Password(string password) => From(password);
    }

    internal class PasswordValidator : AbstractValidator<Password>
    {
        public PasswordValidator()
        {
            RuleFor(p => p.Value)
                .MinimumLength(5)
                .MaximumLength(26)
                .Must(ContainsCapitalLetter)
                .Must(ContainsSpecialCharacter)
                .NotEmpty();
        }

        private static bool ContainsCapitalLetter(string password) => password.Any(char.IsUpper);

        private static bool ContainsSpecialCharacter(string password)
        {
            char[] special = {'$', '%', '@', '_', '&', '^', '*', '!', '#'};
            return password.Any(special.Contains);
        }
    }
}