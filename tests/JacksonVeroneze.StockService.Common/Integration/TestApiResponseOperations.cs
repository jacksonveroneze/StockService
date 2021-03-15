namespace JacksonVeroneze.StockService.Common.Integration
{
    public class TestApiResponseOperations<T> : TestApiResponseError where T : class
    {
        public T Data { get; set; }
    }
}
