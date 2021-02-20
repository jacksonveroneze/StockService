using Bogus;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class AddOrUpdatePurchaseItemDtoFaker
    {
        public static Faker<AddOrUpdatePurchaseItemDto> GenerateValidFaker()
        {
            return new Faker<AddOrUpdatePurchaseItemDto>()
                .RuleFor(x => x.ProductId, f => f.Random.Guid())
                .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
                .RuleFor(x => x.Value, f => f.Random.Decimal(1, 100));
        }
    }
}
