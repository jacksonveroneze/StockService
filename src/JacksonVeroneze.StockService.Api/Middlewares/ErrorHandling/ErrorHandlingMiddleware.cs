using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Core.DomainObjects;
using JacksonVeroneze.StockService.Core.DomainObjects.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ApplicationException = JacksonVeroneze.StockService.Core.DomainObjects.Exceptions.ApplicationException;

namespace JacksonVeroneze.StockService.Api.Middlewares.ErrorHandling
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
            catch (NotFoundException e)
            {
                await FactoryResponse(context, e, HttpStatusCode.NotFound);
            }
            catch (Exception e) when (e is DomainException || e is ApplicationException)
            {
                await FactoryResponse(context, e, HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                await FactoryResponse(context, e, HttpStatusCode.InternalServerError);
            }
        }

        private async Task FactoryResponse(HttpContext context, Exception e, HttpStatusCode statusCode)
        {
            Error resultReturn = new Error
            {
                Status = (int)statusCode,
                Message = e.Message,
                Trace = (_hostEnvironment.IsDevelopment() ? e.StackTrace : string.Empty)
            };

            JsonSerializerOptions serializeOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true
            };

            string result = JsonSerializer.Serialize(resultReturn, serializeOptions);

            context.Response.ContentType = "application/json; charset=utf-8";

            context.Response.StatusCode = (int)statusCode;

            _logger.LogError(result);

            await context.Response.WriteAsync(result);
        }
    }
}
