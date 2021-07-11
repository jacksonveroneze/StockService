using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class DevolutionFaker
    {
        public static Devolution Generate()
            => FakerData().Generate();

        public static IList<Devolution> Generate(int total)
            => FakerData().Generate(total);

        public static Devolution GenerateWithItems(int total)
        {
            Devolution output = FakerData().Generate();

            for (int i = 0; i < total; i++)
                output.AddItem(DevolutionItemFaker.Generate(output, ProductFaker.Generate()));

            return output;
        }

        public static Devolution GenerateWithItem(Product product)
        {
            Devolution output = FakerData().Generate();

            output.AddItem(DevolutionItemFaker.Generate(output, product));

            return output;
        }

        private static Faker<Devolution> FakerData()
            => new Faker<Devolution>()
                .CustomInstantiator(f =>
                    new Devolution(
                        f.Commerce.Product(),
                        f.Date.Recent()
                    )
                );
    }
}
