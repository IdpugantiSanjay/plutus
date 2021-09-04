using System;
using FluentValidation;
using Plutus.Domain.Enums;
using Plutus.Domain.Interfaces;
using Plutus.Domain.ValueObjects;

namespace Plutus.Domain
{
    public class Transaction: IAuditableEntity
    {
        public Amount Amount { get; private set; }
        public Username Username { get; private set; }
        public TransactionDateTime DateTime { get; private set; }
        public TransactionDescription? Description { get; private set; }
        public Guid CategoryId { get; private set; }
        public Guid Id { get; }
        public TransactionType TransactionType { get; private set; }

        public User User { get; }
        
        public Category Category { get;  }
        
        private Transaction()
        {
            
        }
        
        public Transaction(Guid id, TransactionType type, Username username, Amount amount, TransactionDateTime dateTime, Guid categoryId, TransactionDescription? description)
        {
            Id = id;
            TransactionType = type;
            Username = username;
            Amount = amount;
            DateTime = dateTime;
            CategoryId = categoryId;
            Description = description;
        }
        public void Update(Transaction transaction)
        {
            Amount = transaction.Amount;
            Username = transaction.Username;
            DateTime = transaction.DateTime;
            Description = transaction.Description;
            CategoryId = transaction.CategoryId;
            TransactionType = transaction.TransactionType;
            
            LastModifiedUtc = System.DateTime.UtcNow;
        }

        public void MakeInActive()
        {
            InActive = true;
            LastModifiedUtc = System.DateTime.UtcNow;
        }
        
        #region AUDITABLE PROPERTIES
        public DateTime LastModifiedUtc { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        
        public bool InActive { get; set; }
        #endregion
    }
}