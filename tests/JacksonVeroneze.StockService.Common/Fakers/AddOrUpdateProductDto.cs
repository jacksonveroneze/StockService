using Bogus;
using JacksonVeroneze.StockService.Application.DTO.Product;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class AddOrUpdateProductDtoFaker
    {
        public static Faker<AddOrUpdateProductDto> GenerateValidFaker()
        {
            return new Faker<AddOrUpdateProductDto>()
                .RuleFor(x => x.Description, f => f.Commerce.Product())
                .RuleFor(x => x.IsActive, true);
        }

        public static Faker<AddOrUpdateProductDto> GenerateInvalidFaker()
        {
            return new Faker<AddOrUpdateProductDto>()
                .RuleFor(x => x.Description, string.Empty)
                .RuleFor(x => x.IsActive, true);
        }
    }
}
