using System;

namespace Plutus.Application.Exceptions
{
    public class UsernameTakenException : Exception
    {
        public string Field { get; }

        public UsernameTakenException(string username): base($"Username: {username} already taken")
        {
            Field = "Username";
        }
    }
}