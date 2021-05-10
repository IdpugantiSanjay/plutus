using System.Linq;
using FluentValidation;
using Plutus.Domain.Exceptions;
using ValueOf;

namespace Plutus.Domain.ValueObjects
{
    public class Password : ValueOf<string, Password>
    {
        public static readonly PasswordValidator Validator = new();

        protected override void Validate()
        {
            if (Value is null || Validator.Validate(Value).IsValid is false)
                throw new InvalidPasswordException();
        }
        
        public static implicit operator Password(string password) => From(password);
    }

    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(p => p)
                .MinimumLength(5)
                // .MaximumLength(26)
                // .Must(ContainsCapitalLetter)
                // .Must(ContainsSpecialCharacter)
                .NotEmpty();
        }

        // private static bool ContainsCapitalLetter(string password) => password.Any(char.IsUpper);
        //
        // private static bool ContainsSpecialCharacter(string password)
        // {
        //     char[] special = {'$', '%', '@', '_', '&', '^', '*', '!', '#'};
        //     return password.Any(special.Contains);
        // }
    }
}