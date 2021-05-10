using System;
using Plutus.Domain.Enums;

namespace Plutus.Application.Categories.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}