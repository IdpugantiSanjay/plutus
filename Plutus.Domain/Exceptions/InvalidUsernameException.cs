using System;

namespace Plutus.Domain.Exceptions
{
    public class InvalidUsernameException : Exception
    {
        public string Field { get; }

        public InvalidUsernameException(string message): base(message)
        {
            Field = "Username";
        }
    }
}