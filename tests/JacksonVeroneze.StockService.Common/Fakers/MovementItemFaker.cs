using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class MovementItemFaker
    {
        public static Faker<MovementItem> GenerateFaker(Movement movement)
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
