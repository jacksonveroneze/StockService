using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class ProductFaker
    {
        public static Product Generate()
            => FakerData().Generate();

        public static IList<Product> Generate(int totalItems)
            => FakerData().Generate(totalItems);

        private static Faker<Product> FakerData()
            => new Faker<Product>()
                .CustomInstantiator(f =>
                    new Product(
                        $"{f.Commerce.Product()} - {f.Commerce.ProductName()}"
                    )
                );
    }
}
