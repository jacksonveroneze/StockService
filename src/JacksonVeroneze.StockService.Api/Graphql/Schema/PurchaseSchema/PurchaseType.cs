using GraphQL.Types;
using JacksonVeroneze.StockService.Application.DTO.Purchase;

namespace JacksonVeroneze.StockService.Api.Graphql.Schema.PurchaseSchema
{
    public sealed class PurchaseType : ObjectGraphType<PurchaseDto>
    {
        public PurchaseType()
        {
            Name = "Purchase";
            Description = "Purchase Type";

            Field(x => x.Id);
            Field(x => x.Description);
            Field(x => x.Date);
            Field(x => x.TotalValue);
            Field(x => x.State);
        }
    }
}
