using System;
using GraphQL.Utilities;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema
{
    public class StockSchema : GraphQL.Types.Schema
    {
        public StockSchema(IServiceProvider resolver) : base(resolver)
        {
            Query = resolver.GetRequiredService<QueryType>();
            Mutation = resolver.GetRequiredService<MutationType>();
        }
    }
}
