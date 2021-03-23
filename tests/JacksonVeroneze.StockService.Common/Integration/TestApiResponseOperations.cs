namespace JacksonVeroneze.StockService.Common.Integration
{
    public sealed class TestApiResponseOperations<T> : TestApiResponseError where T : class
    {
        public T Content { get; set; }
    }
}
