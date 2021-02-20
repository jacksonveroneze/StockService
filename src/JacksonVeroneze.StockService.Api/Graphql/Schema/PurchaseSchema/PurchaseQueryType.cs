using System;
using GraphQL;
using GraphQL.Types;
using JacksonVeroneze.StockService.Application.Services;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema
{
    public class PurchaseQueryType : ObjectGraphType
    {
        public PurchaseQueryType(PurchaseApplicationService service)
        {
            Name = "UserQuery";
            Description = "User Query Type";

            Field<ListGraphType<PurchaseType>>(
                "allPurchases",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "skip"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "take"}
                ),
                resolve: _ => service.FindAllAsync());

            Field<PurchaseType>(
                "Purchase",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> {Name = "id"}
                ),
                resolve: context => service.FindAsync(context.GetArgument<Guid>("id")));
        }
    }
}
