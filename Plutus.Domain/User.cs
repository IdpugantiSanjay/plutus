using System;
using System.Collections;
using System.Collections.Generic;
using Plutus.Domain.Interfaces;
using Plutus.Domain.ValueObjects;

namespace Plutus.Domain
{
    public class User : IAuditableEntity
    {
        public Username Username { get; }
        public Email Email { get; }
        public Password Password { get; }
        public string FirstName { get; }
        public string LastName { get; }

        private User()
        {
        }

        public User(Username username, Email email, Password password, string firstName, string lastName)
        {
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        public IEnumerable<Transaction> Transactions { get; }

        #region AUDITABLE PROPERTIES

        public DateTime LastModifiedUtc { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool InActive { get; set; }

        #endregion
    }
}