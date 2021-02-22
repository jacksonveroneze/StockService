using System;
using GraphQL;
using GraphQL.Types;
using JacksonVeroneze.StockService.Api.Graphql.Schema.Util;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Domain.Filters;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema
{
    public class PurchaseQueryType : ObjectGraphType
    {
        public PurchaseQueryType(IPurchaseApplicationService service)
        {
            Name = "UserQuery";
            Description = "User Query Type";

            Field<ListGraphType<PurchaseType>>(
                "allPurchases",
                arguments: new QueryArguments(
                    new QueryArgument<PurchasePaginateInputType> {Name = Constants.Paginate},
                    new QueryArgument<PurchaseFilterInputType> {Name = Constants.Filter}
                ),
                resolve: context =>
                {
                    Pagination pagination = context.GetArgument<Pagination>(Constants.Paginate);
                    PurchaseFilter filter = context.GetArgument<PurchaseFilter>(Constants.Filter);

                    return service.FilterAsync(pagination ??= new Pagination(), filter ??= new PurchaseFilter());
                });

            Field<PurchaseType>(
                "purchase",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> {Name = Constants.Id}
                ),
                resolve: context => service.FindAsync(context.GetArgument<Guid>(Constants.Id)));
        }
    }
}
