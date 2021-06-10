using System;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Core.Data;
using JacksonVeroneze.StockService.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers.v1
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
        [Authorize("products:filter")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<Pageable<ProductDto>>> Filter(
            [FromQuery] Pagination pagination,
            [FromQuery] ProductFilter filter)
            => Ok(await _applicationService.FilterAsync(pagination, filter));

        /// <summary>
        /// Method responsible for action: Find.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [Authorize("products:find")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<ProductDto>> Find(Guid id)
        {
            ProductDto productDto = await _applicationService.FindAsync(id);

            if (productDto is null)
                return NotFound(FactoryNotFound());

            return Ok(productDto);
        }

        /// <summary>
        /// Method responsible for action: Create.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("products:create")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<ProductDto>> Create([FromBody] AddOrUpdateProductDto productDto)
        {
            ApplicationDataResult<ProductDto> result = await _applicationService.AddAsync(productDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return CreatedAtAction(nameof(Find), new {id = result.Data.Id}, result);
        }

        /// <summary>
        /// Method responsible for action: Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Authorize("products:update")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<ActionResult<ProductDto>> Update(Guid id, [FromBody] AddOrUpdateProductDto productDto)
        {
            ApplicationDataResult<ProductDto> result = await _applicationService.UpdateAsync(id, productDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return Ok(result);
        }

        /// <summary>
        /// Method responsible for action: Delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Authorize("products:delete")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            ApplicationDataResult<ProductDto> result = await _applicationService.RemoveAsync(id);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return NoContent();
        }
    }
}
