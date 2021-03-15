using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Api.Util;
using JacksonVeroneze.StockService.Core.DomainObjects.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            catch (Exception e) when (e is DomainException)
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
            ProblemDetails problemDetails =
                FactoryProblemDetailsApi.Factory(context.Request, statusCode, e);

            JsonSerializerOptions serializeOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true
            };

            string result = JsonSerializer.Serialize(problemDetails, serializeOptions);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problemDetails.Status.Value;

            _logger.LogError(result);

            await context.Response.WriteAsync(result);
        }
    }
}
