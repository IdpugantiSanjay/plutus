using System;
using System.Threading;
using System.Threading.Tasks;
using Nest;

namespace Plutus.Application.Transactions.Indexes
{
    public class TransactionIndex
    {
        public record Transaction(Guid Id, string Username, string Category, DateTime DateTime, decimal Amount,
            string Description, bool IsCredit);
        
        private readonly IElasticClient _client;

        public TransactionIndex(IElasticClient client)
        {
            _client = client;
        }

        public async Task<string> IndexAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            var response = await _client.IndexDocumentAsync(transaction, cancellationToken);
            return response.Result switch
            {
                Result.Created => response.Id,
                _ => throw new InvalidOperationException()
            };
        }

        public async Task<string> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var response = await _client.DeleteAsync<Transaction>(id, null, cancellationToken);
            
            return response.Result switch
            {
                Result.Deleted => response.Id,
                _ => throw new InvalidOperationException()
            };
        }
    }
}