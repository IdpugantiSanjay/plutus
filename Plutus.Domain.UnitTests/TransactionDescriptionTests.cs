using System;
using System.Threading.Channels;
using Bogus;
using Plutus.Domain.ValueObjects;
using Xunit;

namespace Plutus.Domain.UnitTests
{
    public class TransactionDescriptionTests
    {
        [Fact]
        public void CanBeEmpty()
        {
            TransactionDescription description = "";
            Assert.Equal("", description);
        }
        
        [Fact]
        public void CanBeNull()
        {
            TransactionDescription description = null;
            Assert.Null(description);
        }
        
        [Fact]
        public void LengthCannotBeMoreThan1024()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var description = string.Join("", new Faker().Random.Chars(count: 1025)) ;
                TransactionDescription.From(description);
            });
        }
    }
}