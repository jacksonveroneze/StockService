using System;
using System.Collections.Generic;
using System.Net;
using JacksonVeroneze.StockService.Core.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace JacksonVeroneze.StockService.Api.Util
{
    public class ProblemDetailsApi : ProblemDetails
    {
        public List<Notification> Errors { get; set; }
    }

    public static class FactoryProblemDetailsApi
    {
        public static ProblemDetailsApi Factory(HttpRequest request, HttpStatusCode statusCode)
        {
            return new()
            {
                Instance = request.HttpContext.Request.Path,
                Title = "The request is invalid",
                Status = (int)statusCode
            };
        }

        public static ProblemDetailsApi Factory(HttpRequest request, HttpStatusCode statusCode,
            List<Notification> errors)
        {
            return new()
            {
                Instance = request.HttpContext.Request.Path,
                Title = "The request is invalid",
                Status = (int)statusCode,
                Errors = errors
            };
        }

        public static ProblemDetailsApi Factory(HttpRequest request, HttpStatusCode statusCode, Exception e,
            IHostEnvironment hostEnvironment)
        {
            return new()
            {
                Instance = request.HttpContext.Request.Path,
                Title = e.Message,
                Status = (int)statusCode,
                Detail = hostEnvironment.IsDevelopment() ? e.StackTrace : string.Empty
            };
        }
    }
}
