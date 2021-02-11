using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class PurchaseFaker
    {
        public static Faker<Purchase> GenerateFaker()
        {
            return new Faker<Purchase>()
                .CustomInstantiator(f =>
                    new Purchase(
                        f.Commerce.Product(),
                        f.Date.Recent()
                    )
                );
        }
    }
}
