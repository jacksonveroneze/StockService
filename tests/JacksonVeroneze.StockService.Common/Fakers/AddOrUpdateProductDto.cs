using Bogus;
using JacksonVeroneze.StockService.Application.DTO.Product;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AddOrUpdateProductDtoFaker
    {
        public static AddOrUpdateProductDto GenerateValid()
        {
            return new Faker<AddOrUpdateProductDto>()
                .RuleFor(x => x.Description, f => f.Commerce.Product())
                .RuleFor(x => x.IsActive, true)
                .Generate();
        }

        public static AddOrUpdateProductDto GenerateInvalid()
        {
            return new Faker<AddOrUpdateProductDto>()
                .RuleFor(x => x.Description, string.Empty)
                .RuleFor(x => x.IsActive, true)
                .Generate();
        }
    }
}
