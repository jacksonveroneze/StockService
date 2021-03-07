using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class OutputItemFaker
    {
        public static Faker<OutputItem> GenerateFaker(Output output)
        {
            return new Faker<OutputItem>()
                .CustomInstantiator(f =>
                    new OutputItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        output,
                        ProductFaker.GenerateFaker().Generate()
                    )
                );
        }

        public static Faker<OutputItem> GenerateFaker(Output output, Product product)
        {
            return new Faker<OutputItem>()
                .CustomInstantiator(f =>
                    new OutputItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        output,
                        product
                    )
                );
        }
    }
}
