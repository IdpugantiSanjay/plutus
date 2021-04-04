using System;
using Plutus.Domain.Interfaces;
using Plutus.Domain.ValueObjects;

namespace Plutus.Domain
{
    public class User: IAuditableEntity
    {
        public Username Username { get; private set; }
        public Email Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Password Password { get; private set; }

        private User()
        {
            
        }
        
        public User(Username username, Email email, string firstName, string lastName, Password password)
        {
            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }
        
        #region AUDITABLE PROPERTIES
        public DateTime LastModifiedUtc { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool InActive { get; set; }
        #endregion
    }
}