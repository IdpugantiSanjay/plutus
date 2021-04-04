using System;
using Plutus.Domain.Exceptions;
using Plutus.Domain.ValueObjects;
using Xunit;

namespace Plutus.Domain.UnitTests
{
    public class UsernameTests
    {
        [Fact]
        public void WhenUsernameContainsCapitalLetter()
        {
            Assert.Throws<InvalidUsernameException>(() => Username.From("Capital-letter"));
        }

        [Fact]
        public void WhenUsernameContainsSpace()
        {
            Assert.Throws<InvalidUsernameException>(() => Username.From("space inbetween"));
        }

        [Fact]
        public void WhenUsernameContainsLowerCaseLetters()
        {
            string username = Username.From("lowercase");
            Assert.Equal("lowercase", username);
        }
    }
}