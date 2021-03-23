using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class PurchaseItemFaker
    {
        public static PurchaseItem Generate(Purchase purchase)
            => FakerData(purchase, ProductFaker.Generate()).Generate();

        public static PurchaseItem Generate(Purchase purchase, Product product)
            => FakerData(purchase, product).Generate();

        public static IList<PurchaseItem> Generate(Purchase purchase, int total)
            => FakerData(purchase).Generate(total);

        private static Faker<PurchaseItem> FakerData(Purchase purchase, Product product)
            => new Faker<PurchaseItem>()
                .CustomInstantiator(f =>
                    new PurchaseItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        purchase,
                        product
                    )
                );

        private static Faker<PurchaseItem> FakerData(Purchase purchase)
            => new Faker<PurchaseItem>()
                .CustomInstantiator(f =>
                    new PurchaseItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        purchase,
                        ProductFaker.Generate()
                    )
                );
    }
}
