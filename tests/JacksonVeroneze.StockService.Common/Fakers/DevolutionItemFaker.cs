using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class DevolutionItemFaker
    {
        public static DevolutionItem Generate(Devolution devolution)
            => FakerData(devolution, ProductFaker.Generate(), 100).Generate();

        public static DevolutionItem Generate(Devolution devolution, Product product)
            => FakerData(devolution, product, 100).Generate();

        public static DevolutionItem Generate(Devolution devolution, Product product, int maxAmmount)
            => FakerData(devolution, product, maxAmmount).Generate();

        public static IList<DevolutionItem> Generate(Devolution devolution, int total)
            => FakerData(devolution).Generate(total);

        private static Faker<DevolutionItem> FakerData(Devolution devolution, Product product, int maxAmmount)
            => new Faker<DevolutionItem>()
                .CustomInstantiator(f =>
                    new DevolutionItem(
                        f.Random.Int(1, maxAmmount),
                        devolution,
                        product
                    )
                );

        private static Faker<DevolutionItem> FakerData(Devolution devolution)
            => new Faker<DevolutionItem>()
                .CustomInstantiator(f =>
                    new DevolutionItem(
                        f.Random.Int(1, 100),
                        devolution,
                        ProductFaker.Generate()
                    )
                );
    }
}
