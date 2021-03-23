using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public static class MovementItemFaker
    {
        public static Faker<MovementItem> Generate(Movement movement)
        {
            return new Faker<MovementItem>()
                .CustomInstantiator(f =>
                    new MovementItem(
                        f.Random.Int(1, 100),
                        movement
                    )
                );
        }
    }
}
