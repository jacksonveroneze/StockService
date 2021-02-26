using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductApplicationService _applicationService;

        public ProductsController(IProductApplicationService applicationService)
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
            [FromQuery] ProductFilter filter)
            => Ok(await _applicationService.FilterAsync(pagination, filter));

        //
        // Summary:
        //     /// Method responsible for action: Find. ///
        //
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<ProductDto>> Find(Guid id)
            => Ok(await _applicationService.FindAsync(id));

        //
        // Summary:
        //     /// Method responsible for action: Add. ///
        //
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<ProductDto>> Add([FromBody] AddOrUpdateProductDto productDto)
        {
            ApplicationDataResult<ProductDto> result = await _applicationService.AddASync(productDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Find), new {id = result.Data.Id}, result.Data);
        }

        //
        // Summary:
        //     /// Method responsible for action: Update. ///
        //
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] AddOrUpdateProductDto purchaseDto)
        {
            ApplicationDataResult<ProductDto> result = await _applicationService.UpdateASync(id, purchaseDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        //
        // Summary:
        //     /// Method responsible for action: Delete. ///
        //
        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _applicationService.RemoveASync(id);

            return NoContent();
        }
    }
}
