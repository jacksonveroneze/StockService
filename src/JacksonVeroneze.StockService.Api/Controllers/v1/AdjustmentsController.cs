using System;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Adjustment;
using JacksonVeroneze.StockService.Application.DTO.AdjustmentItem;
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
    public class AdjustmentsController : Controller
    {
        private readonly IAdjustmentApplicationService _applicationService;

        /// <summary>
        /// Method responsible for initialize controller.
        /// </summary>
        /// <param name="applicationService"></param>
        public AdjustmentsController(IAdjustmentApplicationService applicationService)
            => _applicationService = applicationService;

        /// <summary>
        /// Method responsible for action: Filter.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("adjustments:filter")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<Pageable<AdjustmentDto>>> Filter(
            [FromQuery] Pagination pagination,
            [FromQuery] AdjustmentFilter filter)
            => Ok(await _applicationService.FilterAsync(pagination, filter));

        /// <summary>
        /// Method responsible for action: Filter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize("adjustments:find")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<AdjustmentDto>> Find(Guid id)
        {
            AdjustmentDto adjustmentDto = await _applicationService.FindAsync(id);

            if (adjustmentDto is null)
                return NotFound();

            return Ok(adjustmentDto);
        }

        /// <summary>
        /// Method responsible for action: Create.
        /// </summary>
        /// <param name="adjustmentDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("adjustments:create")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<AdjustmentDto>> Create([FromBody] AddOrUpdateAdjustmentDto adjustmentDto)
        {
            ApplicationDataResult<AdjustmentDto> result = await _applicationService.AddAsync(adjustmentDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Find), new {id = result.Data.Id}, result.Data);
        }

        /// <summary>
        /// Method responsible for action: Delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize("adjustments:delete")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            ApplicationDataResult<AdjustmentDto> result = await _applicationService.RemoveAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return NoContent();
        }

        /// <summary>
        /// Method responsible for action: Close.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/close")]
        [Authorize("adjustments:close")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Close(Guid id)
        {
            ApplicationDataResult<AdjustmentDto> result = await _applicationService.CloseAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return NoContent();
        }

        /// <summary>
        /// Method responsible for action: FindItems.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/items")]
        [Authorize("adjustments:find-items")]
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
        [Authorize("adjustments:find-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> FindItem(Guid id, Guid itemId)
        {
            AdjustmentItemDto result = await _applicationService.FindItemAsync(id, itemId);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Method responsible for action: CreateItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adjustmentItemDto"></param>
        /// <returns></returns>
        [HttpPost("{id}/items")]
        [Authorize("adjustments:create-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<AdjustmentDto>> CreateItem(Guid id,
            [FromBody] AddOrUpdateAdjustmentItemDto adjustmentItemDto)
        {
            ApplicationDataResult<AdjustmentItemDto> result =
                await _applicationService.AddItemAsync(id, adjustmentItemDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(FindItem),
                new {id = result.Data.AdjustmentId, itemId = result.Data.Id}, result.Data);
        }

        /// <summary>
        /// Method responsible for action: UpdateItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <param name="adjustmentItemDto"></param>
        /// <returns></returns>
        [HttpPut("{id}/items/{itemId}")]
        [Authorize("adjustments:update-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<ActionResult<AdjustmentDto>> UpdateItem(Guid id, Guid itemId,
            [FromBody] AddOrUpdateAdjustmentItemDto adjustmentItemDto)
        {
            ApplicationDataResult<AdjustmentItemDto> result =
                await _applicationService.UpdateItemAsync(id, itemId, adjustmentItemDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        /// <summary>
        /// Method responsible for action: DeleteItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpDelete("{id}/items/{itemId}")]
        [Authorize("adjustments:remove-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> RemoveItem(Guid id, Guid itemId)
        {
            ApplicationDataResult<AdjustmentItemDto> result = await _applicationService.RemoveItemAsync(id, itemId);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}
