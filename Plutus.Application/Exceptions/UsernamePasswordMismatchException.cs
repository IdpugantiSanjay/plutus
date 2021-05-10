using System;

namespace Plutus.Application.Exceptions
{
    public class UsernamePasswordMismatchException: Exception
    {
        public UsernamePasswordMismatchException(): base("Username and Password does not match")
        {
            
        }
    }
}