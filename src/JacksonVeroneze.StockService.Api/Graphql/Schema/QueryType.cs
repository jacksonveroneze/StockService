using GraphQL.Types;
using JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema
{
    public class QueryType : ObjectGraphType
    {
        public QueryType()
        {
            Name = "Query";

            Field<PurchaseQueryType>("purchaseRootQuery", resolve: _ => new { });
        }
    }
}
