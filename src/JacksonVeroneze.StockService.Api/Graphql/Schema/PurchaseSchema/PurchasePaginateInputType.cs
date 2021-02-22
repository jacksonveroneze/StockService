using GraphQL.Types;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema
{
    public sealed class PurchasePaginateInputType : InputObjectGraphType<Pagination>
    {
        public PurchasePaginateInputType()
        {
            Name = "PurchasePaginate";
            Description = "Purchase Paginate Type";

            Field(x => x.Skip, true).DefaultValue(0);
            Field(x => x.Take, true).DefaultValue(0);
        }
    }
}
