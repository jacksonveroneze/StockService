namespace JacksonVeroneze.StockService.Common.Integration
{
    public sealed class TestApiResponseOperationGet<T> : TestApiResponseError
    {
        public T Content { get; set; }
    }
}
