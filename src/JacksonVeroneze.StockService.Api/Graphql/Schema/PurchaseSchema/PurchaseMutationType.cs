using System.Linq;
using GraphQL;
using GraphQL.Types;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema
{
    public class PurchaseMutationType : ObjectGraphType
    {
        //
        // Summary:
        //     /// Método responsável por inicializar a classe. ///
        //
        public PurchaseMutationType(IPurchaseApplicationService service)
        {
            Name = "PurchaseMutation";
            Description = "Purchase Mutation Type";

            FieldAsync<PurchaseType>(
                "createPurchase",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PurchaseInputType>> {Name = "data"}
                ),
                resolve: async context =>
                {
                    AddOrUpdatePurchaseDto data = context.GetArgument<AddOrUpdatePurchaseDto>("data");

                    ApplicationDataResult<PurchaseDto> result = await service.AddAsync(data);

                    if (result.IsSuccess)
                        return result.Data;

                    context.Errors.AddRange(result.Errors.Select(x => new ExecutionError(x)));

                    return null;
                }
            );
        }
    }
}
