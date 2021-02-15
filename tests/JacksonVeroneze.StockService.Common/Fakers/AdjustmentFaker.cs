using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class AdjustmentFaker
    {
        public static Faker<Adjustment> GenerateFaker()
        {
            return new Faker<Adjustment>()
                .CustomInstantiator(f =>
                    new Adjustment(
                        f.Commerce.Product(),
                        f.Date.Recent()
                    )
                );
        }
    }
}
