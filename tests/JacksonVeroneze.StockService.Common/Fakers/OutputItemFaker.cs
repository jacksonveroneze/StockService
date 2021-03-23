using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class OutputItemFaker
    {
        public static OutputItem Generate(Output output)
            => FakerData(output, ProductFaker.Generate()).Generate();

        public static OutputItem Generate(Output output, Product product)
            => FakerData(output, product).Generate();

        public static IList<OutputItem> Generate(Output output, int total)
            => FakerData(output).Generate(total);

        private static Faker<OutputItem> FakerData(Output output, Product product)
            => new Faker<OutputItem>()
                .CustomInstantiator(f =>
                    new OutputItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        output,
                        product
                    )
                );

        private static Faker<OutputItem> FakerData(Output output)
            => new Faker<OutputItem>()
                .CustomInstantiator(f =>
                    new OutputItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        output,
                        ProductFaker.Generate()
                    )
                );
    }
}
