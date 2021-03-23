using System.Collections.Generic;
using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class AdjustmentItemFaker
    {
        public static AdjustmentItem Generate(Adjustment adjustment)
            => FakerData(adjustment, ProductFaker.Generate()).Generate();

        public static AdjustmentItem Generate(Adjustment adjustment, Product product)
            => FakerData(adjustment, product).Generate();

        public static IList<AdjustmentItem> Generate(Adjustment adjustment, int total)
            => FakerData(adjustment).Generate(total);

        private static Faker<AdjustmentItem> FakerData(Adjustment adjustment, Product product)
            => new Faker<AdjustmentItem>()
                .CustomInstantiator(f =>
                    new AdjustmentItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        adjustment,
                        product
                    )
                );

        private static Faker<AdjustmentItem> FakerData(Adjustment adjustment)
            => new Faker<AdjustmentItem>()
                .CustomInstantiator(f =>
                    new AdjustmentItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        adjustment,
                        ProductFaker.Generate()
                    )
                );
    }
}
