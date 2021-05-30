using System;
using Bogus;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AddOrUpdateOutputItemDtoFaker
    {
        public static AddOrUpdateOutputItemDto GenerateValid(Guid productId)
        {
            return new Faker<AddOrUpdateOutputItemDto>()
                .RuleFor(x => x.ProductId, productId)
                .RuleFor(x => x.Amount, f => f.Random.Int(1, 100))
                .Generate();
        }
    }
}
