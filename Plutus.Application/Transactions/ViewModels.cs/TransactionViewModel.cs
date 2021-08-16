using System;
using Plutus.Application.Categories.ViewModels;
using Plutus.Domain;
using Plutus.Domain.Enums;

namespace Plutus.Application.Transactions.ViewModels.cs
{
    public class TransactionViewModel
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public DateTime DateTime { get; init; }

        public string Category { get; init; }

        public decimal Amount { get; set; }
        
        public TransactionType TransactionType { get; init; }
        public string Description { get; init; }
    }
}