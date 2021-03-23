using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class MovementFaker
    {
        public static Faker<Movement> Generate()
        {
            return new Faker<Movement>()
                .CustomInstantiator(f =>
                    new Movement(ProductFaker.Generate())
                );
        }
    }
}
