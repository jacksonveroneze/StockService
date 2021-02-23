using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using JacksonVeroneze.StockService.Api.Graphql.Schema.Util;
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
                "addPurchase",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PurchaseInputType>> {Name = Constants.Data}
                ),
                resolve: async context =>
                {
                    AddOrUpdatePurchaseDto data = context.GetArgument<AddOrUpdatePurchaseDto>(Constants.Data);

                    ApplicationDataResult<PurchaseDto> result = await service.AddAsync(data);

                    if (result.IsSuccess)
                        return result.Data;

                    context.Errors.AddRange(result.Errors.Select(x => new ExecutionError(x)));

                    return null;
                }
            );

            FieldAsync<PurchaseType>(
                "updatePurchase",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GuidGraphType>> {Name = Constants.Id},
                    new QueryArgument<NonNullGraphType<PurchaseInputType>> {Name = Constants.Data}
                ),
                resolve: async context =>
                {
                    Guid id = context.GetArgument<Guid>(Constants.Id);
                    AddOrUpdatePurchaseDto data = context.GetArgument<AddOrUpdatePurchaseDto>(Constants.Data);

                    ApplicationDataResult<PurchaseDto> result = await service.UpdateAsync(id, data);

                    if (result.IsSuccess)
                        return result.Data;

                    context.Errors.AddRange(result.Errors.Select(x => new ExecutionError(x)));

                    return null;
                }
            );

            FieldAsync<BooleanGraphType>(
                "removePurchase",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GuidGraphType>> {Name = Constants.Id}
                ),
                resolve: async context =>
                {
                    Guid id = context.GetArgument<Guid>(Constants.Id);

                    await service.RemoveAsync(id);

                    return true;
                }
            );

            FieldAsync<BooleanGraphType>(
                "closePurchase",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<GuidGraphType>> {Name = Constants.Id}
                ),
                resolve: async context =>
                {
                    Guid id = context.GetArgument<Guid>(Constants.Id);

                    await service.CloseAsync(id);

                    return true;
                }
            );
        }
    }
}
