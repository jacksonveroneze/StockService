using System;
using Bogus;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AddOrUpdateAdjustmentItemDtoFaker
    {
        public static AddOrUpdateAdjustmentItemDto GenerateValid(Guid productId)
        {
            return new Faker<AddOrUpdateAdjustmentItemDto>()
                .RuleFor(x => x.ProductId, productId)
                .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
                .RuleFor(x => x.Value, f => f.Random.Decimal(1, 100))
                .Generate();
        }
    }
}
