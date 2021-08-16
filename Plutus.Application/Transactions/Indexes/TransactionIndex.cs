using System;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Queries;

namespace Plutus.Application.Transactions.Indexes
{
    public class TransactionIndex
    {
        public record Index(Guid Id, string Username, string Category, DateTime DateTime, decimal Amount,
            string Description, bool IsCredit);

        private readonly IElasticClient _client;
        private readonly ICategoryRepository categoryRepository;

        public TransactionIndex(IElasticClient client, ICategoryRepository categoryRepository)
        {
            _client = client;
            this.categoryRepository = categoryRepository;
        }


        public async Task<IEnumerable<Index>> FindAsync(FindTransactions.Request request)
        {
            QueryBase queryContainer = new TermQuery { Field = new Field(typeof(Index).GetProperty(nameof(Index.Username))), Value = request.Username };

            if (request.From != default && request.To != default)
                queryContainer &= new DateRangeQuery() { Field = new Field(typeof(Index).GetProperty(nameof(Index.DateTime))), GreaterThanOrEqualTo = DateMath.Anchored(request.From), LessThanOrEqualTo = DateMath.Anchored(request.To) };

            if (request.CategoryId != default)
            {
                var category = await categoryRepository.FindByIdAsync(request.CategoryId);
                if (category is not null)
                    queryContainer &= new MatchQuery { Field = new Field(typeof(Index).GetProperty(nameof(Index.Category))), Query = category.Name };
            }

            if (request.Description is { Length: > 0 })
                queryContainer &= new MatchQuery { Field = new Field(typeof(Index).GetProperty(nameof(Index.Description))), Query = request.Description };

            var searchRequest = new SearchRequest<Index>
            {
                Query = queryContainer,
                Size = request.Limit,
                From = request.Skip
            };

            var response = await _client.SearchAsync<Index>(searchRequest);

            return response.Hits.Select(h => h.Source);
        }

        public async Task<string> IndexAsync(Index transaction, CancellationToken cancellationToken)
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
            var response = await _client.DeleteAsync<Index>(id, null, cancellationToken);

            return response.Result switch
            {
                Result.Deleted => response.Id,
                _ => throw new InvalidOperationException()
            };
        }
    }
}