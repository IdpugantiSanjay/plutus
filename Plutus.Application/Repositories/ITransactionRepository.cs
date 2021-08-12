using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plutus.Application.Transactions.Queries;
using Plutus.Domain;

namespace Plutus.Application.Repositories
{
    public interface ITransactionRepository
    {
        public Task<List<Transaction>> FindAsync(FindTransactions.Request request);

        public Task<Transaction> AddAsync(Transaction transaction);

        public Transaction Update(Transaction transaction);

        public Task<Transaction?> FindByIdAsync(Guid id);
        
        public Task SaveChangesAsync();
    }
}