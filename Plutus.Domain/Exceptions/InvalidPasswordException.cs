using System;

namespace Plutus.Domain.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        private new const string Message =
            @"Invalid Password. The Password should contain atlest one Capital Letter and atlest one Special Character and It should be between 5 and 26 characters in length";

        public static string Field => "Password";

        public InvalidPasswordException() : base(Message)
        {
        }
    }
}