namespace JacksonVeroneze.StockService.Api.Middlewares.ErrorHandling
{
    public class Error
    {
        public string Type { get; set; } = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        public string Title { get; set; } = "One or more validation errors occurred.";

        public int Status { get; set; }

        public string Message { get; set; }

        public string Trace { get; set; }
    }
}
