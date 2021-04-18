using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using JacksonVeroneze.NET.Commons.Exceptions;
using JacksonVeroneze.StockService.Api.Util;
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
                FactoryProblemDetailsApi.Factory(context.Request, statusCode, e, _hostEnvironment);

            JsonSerializerOptions serializeOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true
            };

            context.Response.ContentType = "application/problem+json";

            context.Response.StatusCode = problemDetails.Status ??= 500;

            string result = JsonSerializer.Serialize(problemDetails, serializeOptions);

            _logger.LogError(result);

            await context.Response.WriteAsync(result);
        }
    }
}
