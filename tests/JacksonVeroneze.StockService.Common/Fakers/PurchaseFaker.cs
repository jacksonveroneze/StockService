using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class PurchaseFaker
    {
        public static Purchase Generate()
            => FakerData().Generate();

        public static IList<Purchase> Generate(int total)
            => FakerData().Generate(total);

        public static Purchase GenerateWithItems(int total)
        {
            Purchase purchase = FakerData().Generate();

            for (int i = 0; i < total; i++)
                purchase.AddItem(PurchaseItemFaker.Generate(purchase, ProductFaker.Generate()));

            return purchase;
        }

        public static Purchase GenerateWithItem(Product product)
        {
            Purchase purchase = FakerData().Generate();

            purchase.AddItem(PurchaseItemFaker.Generate(purchase, product));

            return purchase;
        }

        private static Faker<Purchase> FakerData()
            => new Faker<Purchase>()
                .CustomInstantiator(f =>
                    new Purchase(
                        f.Commerce.Product(),
                        f.Date.Recent()
                    )
                );
    }
}
