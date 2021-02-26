using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.StockService.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger,
            IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                Error resultReturn = new Error
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Message = e.Message,
                    Trace = (_hostEnvironment.IsDevelopment() ? e.StackTrace : string.Empty)
                };

                string result = JsonSerializer.Serialize(resultReturn);

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                _logger.LogError(result);

                await context.Response.WriteAsync(result);
            }
        }
    }

    public class Error
    {
        public string Type { get; set; } = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        public string Title { get; set; } = "One or more validation errors occurred.";

        public int Status { get; set; }

        public string Message { get; set; }
        public string Trace { get; set; }
    }
}
