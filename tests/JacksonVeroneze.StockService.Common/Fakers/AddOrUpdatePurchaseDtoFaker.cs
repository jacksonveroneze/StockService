using System;
using Bogus;
using JacksonVeroneze.StockService.Application.DTO.Purchase;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AddOrUpdatePurchaseDtoFaker
    {
        public static AddOrUpdatePurchaseDto GenerateValid()
        {
            return new Faker<AddOrUpdatePurchaseDto>()
                .RuleFor(x => x.Description, f => f.Commerce.Product())
                .RuleFor(x => x.Date, f => f.Date.Recent())
                .Generate();
        }

        public static AddOrUpdatePurchaseDto GenerateInvalid()
        {
            return new Faker<AddOrUpdatePurchaseDto>()
                .RuleFor(x => x.Description, string.Empty)
                .RuleFor(x => x.Date, DateTime.Now.AddDays(10))
                .Generate();
        }
    }
}
