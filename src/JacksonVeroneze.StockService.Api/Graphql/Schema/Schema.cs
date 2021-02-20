using System;
using GraphQL.Utilities;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema
{
    public class Schema : GraphQL.Types.Schema
    {
        public Schema(IServiceProvider resolver) : base(resolver)
        {
            Query = resolver.GetRequiredService<QueryType>();
            Mutation = resolver.GetRequiredService<MutationType>();
        }
    }
}
