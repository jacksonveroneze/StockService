using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class OutputItemFaker
    {
        public static OutputItem Generate(Output output)
            => FakerData(output, ProductFaker.Generate(), 100).Generate();

        public static OutputItem Generate(Output output, Product product)
            => FakerData(output, product, 100).Generate();

        public static OutputItem Generate(Output output, Product product, int maxAmmount)
            => FakerData(output, product, maxAmmount).Generate();

        public static IList<OutputItem> Generate(Output output, int total)
            => FakerData(output).Generate(total);

        private static Faker<OutputItem> FakerData(Output output, Product product, int maxAmmount)
            => new Faker<OutputItem>()
                .CustomInstantiator(f =>
                    new OutputItem(
                        f.Random.Int(1, maxAmmount),
                        output,
                        product
                    )
                );

        private static Faker<OutputItem> FakerData(Output output)
            => new Faker<OutputItem>()
                .CustomInstantiator(f =>
                    new OutputItem(
                        f.Random.Int(1, 100),
                        output,
                        ProductFaker.Generate()
                    )
                );
    }
}
