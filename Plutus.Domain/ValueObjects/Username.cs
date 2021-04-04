using ValueOf;
using FluentValidation;
using Plutus.Domain.Exceptions;

namespace Plutus.Domain.ValueObjects
{
    
    public class Username: ValueOf<string, Username>
    {
        private readonly UsernameValidator _validator = new();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidUsernameException"></exception>
        protected override void Validate()
        {
            var result = _validator.Validate(this);
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
    }

    internal class UsernameValidator : AbstractValidator<Username>
    {
        public UsernameValidator()
        {
            RuleFor(u => u.Value).MaximumLength(12).MinimumLength(3).Must(BeLowerCase).Must(NotContainSpace);
        }

        private bool BeLowerCase(string username) => username == username.ToLowerInvariant();

        private bool NotContainSpace(string username) => username.Contains(' ') == false;
    }
}