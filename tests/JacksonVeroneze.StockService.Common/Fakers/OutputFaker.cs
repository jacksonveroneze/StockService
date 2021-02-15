using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class OutputFaker
    {
        public static Faker<Output> GenerateFaker()
        {
            return new Faker<Output>()
                .CustomInstantiator(f =>
                    new Output(
                        f.Commerce.Product(),
                        f.Date.Recent()
                    )
                );
        }
    }
}
