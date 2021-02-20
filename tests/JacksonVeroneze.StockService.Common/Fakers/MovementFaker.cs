using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class MovementFaker
    {
        public static Faker<Movement> GenerateFaker()
        {
            return new Faker<Movement>()
                .CustomInstantiator(f =>
                    new Movement(ProductFaker.GenerateFaker().Generate())
                );
        }
    }
}
