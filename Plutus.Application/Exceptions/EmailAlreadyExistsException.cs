using System;

namespace Plutus.Application.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {

        public string Field { get;  }
        public EmailAlreadyExistsException(string email) : base($"User with email: {email} already exists")
        {
            Field = "Email";
        }
    }
}