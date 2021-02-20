using GraphQL.Types;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema
{
    public class MutationType : ObjectGraphType
    {
        public MutationType()
        {
            Name = "Mutation";
        }
    }
}
