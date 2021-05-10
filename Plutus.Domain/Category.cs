using System;
using System.Collections.Generic;
using Plutus.Domain.Enums;

namespace Plutus.Domain
{
    public class Category
    {
        public Guid Id { get; }

        public string Name { get; }

        public TransactionType TransactionType { get; }


        private Category()
        {
        }

        public Category(Guid id, string name, TransactionType transactionType)
        {
            Id = id;
            Name = name;
            TransactionType = transactionType;
        }

        public IEnumerable<Transaction> Transactions { get; set; }
    }
}