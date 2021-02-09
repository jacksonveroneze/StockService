using Bogus;
using JacksonVeroneze.StockService.Domain.Entities;

namespace JacksonVeroneze.StockService.Domain.Tests.Fakers
{
    public class ProductFaker
    {
        public static Faker<Product> GenerateFaker()
        {
            return new Faker<Product>()
                .CustomInstantiator(f =>
                    new Product(
                        f.Commerce.Product()
                    )
                );
        }
    }
}
