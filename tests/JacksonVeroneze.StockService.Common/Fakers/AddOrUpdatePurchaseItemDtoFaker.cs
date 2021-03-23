using System;
using Bogus;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AddOrUpdatePurchaseItemDtoFaker
    {
        public static AddOrUpdatePurchaseItemDto GenerateValid(Guid productId)
        {
            return new Faker<AddOrUpdatePurchaseItemDto>()
                .RuleFor(x => x.ProductId, productId)
                .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
                .RuleFor(x => x.Value, f => f.Random.Decimal(1, 100))
                .Generate();
        }
    }
}
