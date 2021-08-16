using System;
using System.Runtime.CompilerServices;
using Nest;
using Microsoft.Extensions.DependencyInjection;
using Plutus.ElasticSearch.Infrastructure.Models;

using Trx = Plutus.Application.Transactions.Indexes.TransactionIndex.Index;

namespace Plutus.ElasticSearch.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddElasticSearch(this IServiceCollection services, ConnectionSettings settings)
        {
            var index = "transaction";

            settings
                .DefaultMappingFor<Trx>(
                    t => t.IndexName(index).IdProperty(trx => trx.Id)
                );
            IElasticClient client = new ElasticClient(settings);

            client.Indices.Create(index, c =>  c.Map<Trx>(trx => trx.AutoMap()));
            

            services.AddSingleton(client);
        }

        private static ITypeMapping Map(TypeMappingDescriptor<Trx> t)
        {
            return t.Properties(ps =>
            {
                ps.Number(pss => pss.Name(trx => trx.Amount))
                    .Keyword(pss => pss.Name(trx => trx.Username))
                    .Date(pss => pss.Name(trx => trx.DateTime))
                    .Keyword(pss => pss.Name(trx => trx.IsCredit))
                    .Keyword(pss => pss.Name(trx => trx.Id))
                    .Keyword(pss => pss.Name(trx => trx.Category))
                    .Text(pss => pss.Name(trx => trx.Description))
                    ;
                    
                return ps;
            });
        }
    }
}