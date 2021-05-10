using Bogus;
using Plutus.Domain.Exceptions;
using Plutus.Domain.ValueObjects;
using Xunit;

namespace Plutus.Domain.UnitTests
{
    public class PasswordTests
    {
        private const string ValidPassword = "Sanjay_11!";
        
        [Fact]
        public void CannotBeNullOrEmpty()
        {
            Assert.Throws<InvalidPasswordException>(() => Password.From(""));
            Assert.Throws<InvalidPasswordException>(() => Password.From(null));
        }

        [Fact]
        public void LengthCannotBeLessThan5Characters()
        {
            var password = string.Join("", new Faker("en").Random.Chars(count: 4));
            Assert.Throws<InvalidPasswordException>(() => Password.From(password));
        }
        
        // [Fact]
        // public void LengthCannotBeGreaterThan26Characters()
        // {
        //     var password = string.Join("", new Faker("en").Random.Chars(count: 27));
        //     Assert.Throws<InvalidPasswordException>(() => Password.From(password));
        // }
        
        // [Fact]
        // public void ShouldContainAtLeastOneCapitalLetter()
        // {
        //     const string invalidPassword = "sanjay";
        //     Assert.Throws<InvalidPasswordException>(() => Password.From(invalidPassword));
        //     Assert.Equal(ValidPassword, Password.From(ValidPassword).Value);
        // }
        //
        // [Fact]
        // public void ShouldContainAtLeastOneSpecialCharacter()
        // {
        //     const string invalidPassword = "Sanjay";
        //     Assert.Throws<InvalidPasswordException>(() => Password.From(invalidPassword));
        //     Assert.Equal(ValidPassword, Password.From(ValidPassword).Value);
        // }
    }
}