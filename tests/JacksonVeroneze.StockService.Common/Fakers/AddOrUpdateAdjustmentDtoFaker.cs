using System;
using Bogus;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AddOrUpdateAdjustmentDtoFaker
    {
        public static AddOrUpdateAdjustmentDto GenerateValid()
        {
            return new Faker<AddOrUpdateAdjustmentDto>()
                .RuleFor(x => x.Description, f => f.Commerce.Product())
                .RuleFor(x => x.Date, f => f.Date.Recent())
                .Generate();
        }

        public static AddOrUpdateAdjustmentDto GenerateInvalid()
        {
            return new Faker<AddOrUpdateAdjustmentDto>()
                .RuleFor(x => x.Description, string.Empty)
                .RuleFor(x => x.Date, DateTime.Now.AddDays(10))
                .Generate();
        }
    }
}
