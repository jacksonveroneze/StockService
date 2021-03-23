using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AdjustmentFaker
    {
        public static Adjustment Generate()
            => FakerData().Generate();

        public static IList<Adjustment> Generate(int total)
            => FakerData().Generate(total);

        public static Adjustment GenerateWithItems(int total)
        {
            Adjustment adjustment = FakerData().Generate();

            for (int i = 0; i < total; i++)
                adjustment.AddItem(AdjustmentItemFaker.Generate(adjustment, ProductFaker.Generate()));

            return adjustment;
        }

        public static Adjustment GenerateWithItem(Product product)
        {
            Adjustment adjustment = FakerData().Generate();

            adjustment.AddItem(AdjustmentItemFaker.Generate(adjustment, product));

            return adjustment;
        }

        private static Faker<Adjustment> FakerData()
            => new Faker<Adjustment>()
                .CustomInstantiator(f =>
                    new Adjustment(
                        f.Commerce.Product(),
                        f.Date.Recent()
                    )
                );
    }
}
