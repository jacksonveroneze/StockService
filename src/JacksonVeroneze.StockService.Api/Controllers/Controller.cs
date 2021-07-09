using System.Net;
using JacksonVeroneze.StockService.Api.Util;
using JacksonVeroneze.StockService.Application.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class Controller : ControllerBase
    {
        protected ProblemDetailsApi FactoryBadRequest<T>(ApplicationDataResult<T> result)
            => FactoryProblemDetailsApi.Factory(Request, HttpStatusCode.BadRequest, result.Errors);

        protected ProblemDetailsApi FactoryNotFound()
            => FactoryProblemDetailsApi.Factory(Request, HttpStatusCode.NotFound);
    }
}
