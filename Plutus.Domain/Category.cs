using System;

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
    }
}