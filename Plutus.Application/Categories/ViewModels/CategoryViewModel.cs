using System;
using Plutus.Domain.Enums;

namespace Plutus.Application.Categories.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public TransactionType TransactionType { get; init; }
    }
}