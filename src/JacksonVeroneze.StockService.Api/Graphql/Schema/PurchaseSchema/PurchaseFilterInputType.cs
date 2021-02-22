using GraphQL.Types;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema
{
    public sealed class PurchaseFilterInputType : InputObjectGraphType<PurchaseFilter>
    {
        public PurchaseFilterInputType()
        {
            Name = "PurchaseFilter";
            Description = "Purchase Filter Type";

            Field(x => x.Description, true);
            Field(x => x.DateInitial, true);
            Field(x => x.DateEnd, true);
        }
    }
}
