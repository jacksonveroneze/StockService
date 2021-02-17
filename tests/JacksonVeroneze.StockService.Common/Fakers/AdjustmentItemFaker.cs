using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class AdjustmentItemFaker
    {
        public static Faker<AdjustmentItem> GenerateFaker(Adjustment adjustmentItem)
        {
            return new Faker<AdjustmentItem>()
                .CustomInstantiator(f =>
                    new AdjustmentItem(
                        f.Random.Int(1, 100),
                        f.Random.Decimal(1, 100),
                        adjustmentItem,
                        ProductFaker.GenerateFaker().Generate()
                    )
                );
        }
    }
}
