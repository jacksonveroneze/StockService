using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    /// <summary>
    /// Class responsible for controller
    /// </summary>
    public class ProductsController : Controller
    {
        private readonly IProductApplicationService _applicationService;

        /// <summary>
        /// Method responsible for initialize controller.
        /// </summary>
        /// <param name="applicationService"></param>
        public ProductsController(IProductApplicationService applicationService)
            => _applicationService = applicationService;

        /// <summary>
        /// Method responsible for action: Filter.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Filter(
            [FromQuery] Pagination pagination,
            [FromQuery] ProductFilter filter)
            => Ok(await _applicationService.FilterAsync(pagination, filter));

        /// <summary>
        /// Method responsible for action: Find.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<ProductDto>> Find(Guid id)
            => Ok(await _applicationService.FindAsync(id));

        /// <summary>
        /// Method responsible for action: Add.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method responsible for action: Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method responsible for action: Delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _applicationService.RemoveASync(id);

            return NoContent();
        }
    }
}
