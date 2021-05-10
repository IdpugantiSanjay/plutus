using System;

namespace Plutus.Domain.Interfaces
{
    public interface IAuditableEntity
    {
        public DateTime LastModifiedUtc { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public bool InActive { get; set; }
    }
}