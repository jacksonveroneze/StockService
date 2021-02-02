using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Tests.Fakers
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
                        purchase
                    )
                );
        }
    }
}
