using System;
using Bogus;
using JacksonVeroneze.StockService.Application.DTO.Purchase;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class AddOrUpdatePurchaseDtoFaker
    {
        public static Faker<AddOrUpdatePurchaseDto> GenerateValidFaker()
        {
            return new Faker<AddOrUpdatePurchaseDto>()
                .RuleFor(x => x.Description, f => f.Commerce.Product())
                .RuleFor(x => x.Date, f => f.Date.Recent());
        }

        public static Faker<AddOrUpdatePurchaseDto> GenerateInvalidFaker()
        {
            return new Faker<AddOrUpdatePurchaseDto>()
                .RuleFor(x => x.Description, string.Empty)
                .RuleFor(x => x.Date, DateTime.Now.AddDays(10));
        }
    }
}
