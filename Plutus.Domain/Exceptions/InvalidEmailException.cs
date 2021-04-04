using System;

namespace Plutus.Domain.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public string Field { get; }


        public InvalidEmailException(): base($"Invalid Email")
        {
            Field = "Email";
        }
    }
}