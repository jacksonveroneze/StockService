using GraphQL.Types;
using JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema
{
    public class MutationType : ObjectGraphType
    {
        public MutationType()
        {
            Name = "Mutation";

            Field<PurchaseMutationType>("purchaseMutationType", resolve: _ => new { });
        }
    }
}
