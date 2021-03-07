using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class PurchaseItemFaker
    {
        public static Faker<PurchaseItem> GenerateFaker(Purchase purchase)
        {
            return new Faker<PurchaseItem>()
                .CustomInstantiator(f =>
                    new PurchaseItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        purchase,
                        ProductFaker.GenerateFaker().Generate()
                    )
                );
        }

        public static Faker<PurchaseItem> GenerateFaker(Purchase purchase, Product product)
        {
            return new Faker<PurchaseItem>()
                .CustomInstantiator(f =>
                    new PurchaseItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        purchase,
                        product
                    )
                );
        }
    }
}
