using Bogus;

namespace JacksonVeroneze.StockService.Common.Fakers
{
    public class Util
    {
        public static string GenerateStringFaker(int size)
        {
            return new Faker()
                .Random.String2(size);
        }
    }
}
