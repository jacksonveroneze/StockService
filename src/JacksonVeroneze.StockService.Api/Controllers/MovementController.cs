using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    public class MovementController : Controller
    {
        private readonly IMovementApplicationService _applicationService;

        public MovementController(IMovementApplicationService applicationService)
            => _applicationService = applicationService;

        //
        // Summary:
        //     /// Method responsible for action: Filter. ///
        //
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Filter(
            [FromQuery] Pagination pagination,
            [FromQuery] MovementFilter filter)
            => Ok(await _applicationService.FilterAsync(pagination, filter));
    }
}
