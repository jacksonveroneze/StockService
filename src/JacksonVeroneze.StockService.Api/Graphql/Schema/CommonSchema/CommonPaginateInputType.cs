using GraphQL.Types;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema.CommonSchema
{
    public sealed class CommonPaginateInputType : InputObjectGraphType<Pagination>
    {
        public CommonPaginateInputType()
        {
            Name = "Paginate";
            Description = "Paginate Type";

            Field(x => x.Skip, true);
            Field(x => x.Take, true);
        }
    }
}
