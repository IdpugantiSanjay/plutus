using ValueOf;
using FluentValidation;
using Plutus.Domain.Exceptions;

namespace Plutus.Domain.ValueObjects
{
    
    public class Username: ValueOf<string, Username>
    {
        public static readonly UsernameValidator Validator = new();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidUsernameException"></exception>
        protected override void Validate()
        {
            var result = Validator.Validate(this);
            if (result.IsValid is false)
                 throw new InvalidUsernameException($"Invalid Username. value: {this.Value}, Username should not contain Capital letters");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <exception cref="InvalidUsernameException"></exception>
        /// <returns></returns>
        public static implicit operator Username(string username) => From(username);
        
        public static implicit operator string(Username username) => username.Value;
        
    }

    public class UsernameValidator : AbstractValidator<string>
    {
        public UsernameValidator()
        {
            RuleFor(u => u).MaximumLength(12).MinimumLength(4).Must(BeLowerCase).Must(NotContainSpace).WithMessage("Invalid Username");
        }

        private bool BeLowerCase(string username) => username == username.ToLowerInvariant();

        private bool NotContainSpace(string username) => username.Contains(' ') == false;
    }
}