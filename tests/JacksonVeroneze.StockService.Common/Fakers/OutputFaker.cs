using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class OutputFaker
    {
        public static Output Generate()
            => FakerData().Generate();

        public static IList<Output> Generate(int total)
            => FakerData().Generate(total);

        public static Output GenerateWithItems(int total)
        {
            Output output = FakerData().Generate();

            for (int i = 0; i < total; i++)
                output.AddItem(OutputItemFaker.Generate(output, ProductFaker.Generate()));

            return output;
        }

        public static Output GenerateWithItem(Product product)
        {
            Output output = FakerData().Generate();

            output.AddItem(OutputItemFaker.Generate(output, product));

            return output;
        }

        private static Faker<Output> FakerData()
            => new Faker<Output>()
                .CustomInstantiator(f =>
                    new Output(
                        f.Commerce.Product(),
                        f.Date.Recent()
                    )
                );
    }
}
