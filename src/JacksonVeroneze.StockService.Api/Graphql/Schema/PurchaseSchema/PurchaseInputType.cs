using GraphQL.Types;
using JacksonVeroneze.StockService.Application.DTO.Purchase;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema
{
    public class PurchaseInputType : InputObjectGraphType<AddOrUpdatePurchaseDto>
    {
        public PurchaseInputType()
        {
            Name = "PurchaseInput";
            Description = "Purchase Input Type";

            Field(x => x.Description);
            Field(x => x.Date);
        }
    }
}
