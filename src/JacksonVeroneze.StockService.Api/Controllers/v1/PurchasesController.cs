using System;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
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
    public class PurchasesController : Controller
    {
        private readonly IPurchaseApplicationService _applicationService;

        /// <summary>
        /// Method responsible for initialize controller.
        /// </summary>
        /// <param name="applicationService"></param>
        public PurchasesController(IPurchaseApplicationService applicationService)
            => _applicationService = applicationService;

        /// <summary>
        /// Method responsible for action: Filter.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("purchases:filter")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<Pageable<PurchaseDto>>> Filter(
            [FromQuery] Pagination pagination,
            [FromQuery] PurchaseFilter filter)
            => Ok(await _applicationService.FilterAsync(pagination, filter));

        /// <summary>
        /// Method responsible for action: Filter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize("purchases:find")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<PurchaseDto>> Find(Guid id)
        {
            PurchaseDto purchaseDto = await _applicationService.FindAsync(id);

            if (purchaseDto is null)
                return NotFound(FactoryNotFound());

            return Ok(purchaseDto);
        }

        /// <summary>
        /// Method responsible for action: Create.
        /// </summary>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("purchases:create")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<PurchaseDto>> Create([FromBody] AddOrUpdatePurchaseDto purchaseDto)
        {
            ApplicationDataResult<PurchaseDto> result = await _applicationService.AddAsync(purchaseDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return CreatedAtAction(nameof(Find), new {id = result.Data.Id}, result);
        }

        /// <summary>
        /// Method responsible for action: Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="purchaseDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize("purchases:update")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<ActionResult<PurchaseDto>> Update(Guid id, [FromBody] AddOrUpdatePurchaseDto purchaseDto)
        {
            ApplicationDataResult<PurchaseDto> result = await _applicationService.UpdateAsync(id, purchaseDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return Ok(result);
        }

        /// <summary>
        /// Method responsible for action: Delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize("purchases:delete")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            ApplicationDataResult<PurchaseDto> result = await _applicationService.RemoveAsync(id);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return NoContent();
        }

        /// <summary>
        /// Method responsible for action: Close.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/close")]
        [Authorize("purchases:close")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Close(Guid id)
        {
            ApplicationDataResult<PurchaseDto> result = await _applicationService.CloseAsync(id);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return NoContent();
        }

        /// <summary>
        /// Method responsible for action: FindItems.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/items")]
        [Authorize("purchases:find-items")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> FindItems(Guid id)
            => Ok(await _applicationService.FindItensAsync(id));

        /// <summary>
        /// Method responsible for action: FindItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet("{id}/items/{itemId}")]
        [Authorize("purchases:find-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> FindItem(Guid id, Guid itemId)
        {
            PurchaseItemDto result = await _applicationService.FindItemAsync(id, itemId);

            if (result is null)
                return NotFound(FactoryNotFound());

            return Ok(result);
        }

        /// <summary>
        /// Method responsible for action: CreateItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="purchaseItemDto"></param>
        /// <returns></returns>
        [HttpPost("{id}/items")]
        [Authorize("purchases:create-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<PurchaseDto>> CreateItem(Guid id,
            [FromBody] AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            ApplicationDataResult<PurchaseItemDto> result =
                await _applicationService.AddItemAsync(id, purchaseItemDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return CreatedAtAction(nameof(FindItem),
                new {id = result.Data.PurchaseId, itemId = result.Data.Id}, result);
        }

        /// <summary>
        /// Method responsible for action: UpdateItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <param name="purchaseItemDto"></param>
        /// <returns></returns>
        [HttpPut("{id}/items/{itemId}")]
        [Authorize("purchases:update-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<ActionResult<PurchaseDto>> UpdateItem(Guid id, Guid itemId,
            [FromBody] AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            ApplicationDataResult<PurchaseItemDto> result =
                await _applicationService.UpdateItemAsync(id, itemId, purchaseItemDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return Ok(result);
        }

        /// <summary>
        /// Method responsible for action: RemoveItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/items/{itemId}")]
        [Authorize("purchases:remove-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> RemoveItem(Guid id, Guid itemId)
        {
            ApplicationDataResult<PurchaseItemDto> result = await _applicationService.RemoveItemAsync(id, itemId);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return NoContent();
        }
    }
}
