using System;
using Bogus;
using JacksonVeroneze.StockService.Application.DTO.Output;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AddOrUpdateOutputDtoFaker
    {
        public static AddOrUpdateOutputDto GenerateValid()
        {
            return new Faker<AddOrUpdateOutputDto>()
                .RuleFor(x => x.Description, f => f.Commerce.Product())
                .RuleFor(x => x.Date, f => f.Date.Recent())
                .Generate();
        }

        public static AddOrUpdateOutputDto GenerateInvalid()
        {
            return new Faker<AddOrUpdateOutputDto>()
                .RuleFor(x => x.Description, string.Empty)
                .RuleFor(x => x.Date, DateTime.Now.AddDays(10))
                .Generate();
        }
    }
}
