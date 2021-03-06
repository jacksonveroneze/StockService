using System;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Output;
using JacksonVeroneze.StockService.Application.DTO.OutputItem;
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
    public class OutputsController : Controller
    {
        private readonly IOutputApplicationService _applicationService;

        /// <summary>
        /// Method responsible for initialize controller.
        /// </summary>
        /// <param name="applicationService"></param>
        public OutputsController(IOutputApplicationService applicationService)
            => _applicationService = applicationService;

        /// <summary>
        /// Method responsible for action: Filter.
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("outputs:filter")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<Pageable<OutputDto>>> Filter(
            [FromQuery] Pagination pagination,
            [FromQuery] OutputFilter filter)
            => Ok(await _applicationService.FilterAsync(pagination, filter));

        /// <summary>
        /// Method responsible for action: Filter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [Authorize("outputs:find")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<OutputDto>> Find(Guid id)
        {
            OutputDto outputDto = await _applicationService.FindAsync(id);

            if (outputDto is null)
                return NotFound(FactoryNotFound());

            return Ok(outputDto);
        }

        /// <summary>
        /// Method responsible for action: Create.
        /// </summary>
        /// <param name="outputDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("outputs:create")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<OutputDto>> Create([FromBody] AddOrUpdateOutputDto outputDto)
        {
            ApplicationDataResult<OutputDto> result = await _applicationService.AddAsync(outputDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return CreatedAtAction(nameof(Find), new {id = result.Data.Id}, result);
        }

        /// <summary>
        /// Method responsible for action: Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="outputDto"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Authorize("outputs:update")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<ActionResult<OutputDto>> Update(Guid id, [FromBody] AddOrUpdateOutputDto outputDto)
        {
            ApplicationDataResult<OutputDto> result = await _applicationService.UpdateAsync(id, outputDto);

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
        [Authorize("outputs:delete")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            ApplicationDataResult<OutputDto> result = await _applicationService.RemoveAsync(id);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return NoContent();
        }

        /// <summary>
        /// Method responsible for action: Close.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}/close")]
        [Authorize("outputs:close")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Close(Guid id)
        {
            ApplicationDataResult<OutputDto> result = await _applicationService.CloseAsync(id);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return NoContent();
        }

        /// <summary>
        /// Method responsible for action: FindItems.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}/items")]
        [Authorize("outputs:find-items")]
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
        [HttpGet("{id:guid}/items/{itemId:guid}")]
        [Authorize("outputs:find-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> FindItem(Guid id, Guid itemId)
        {
            OutputItemDto result = await _applicationService.FindItemAsync(id, itemId);

            if (result is null)
                return NotFound(FactoryNotFound());

            return Ok(result);
        }

        /// <summary>
        /// Method responsible for action: CreateItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="outputItemDto"></param>
        /// <returns></returns>
        [HttpPost("{id:guid}/items")]
        [Authorize("outputs:create-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<ActionResult<OutputDto>> CreateItem(Guid id,
            [FromBody] AddOrUpdateOutputItemDto outputItemDto)
        {
            ApplicationDataResult<OutputItemDto> result =
                await _applicationService.AddItemAsync(id, outputItemDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return CreatedAtAction(nameof(FindItem),
                new {id = result.Data.OutputId, itemId = result.Data.Id}, result);
        }

        /// <summary>
        /// Method responsible for action: UpdateItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <param name="outputItemDto"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}/items/{itemId:guid}")]
        [Authorize("outputs:update-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<ActionResult<OutputDto>> UpdateItem(Guid id, Guid itemId,
            [FromBody] AddOrUpdateOutputItemDto outputItemDto)
        {
            ApplicationDataResult<OutputItemDto> result =
                await _applicationService.UpdateItemAsync(id, itemId, outputItemDto);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return Ok(result);
        }

        /// <summary>
        /// Method responsible for action: DeleteItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}/items/{itemId:guid}")]
        [Authorize("outputs:remove-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> DeleteItem(Guid id, Guid itemId)
        {
            ApplicationDataResult<OutputItemDto> result = await _applicationService.RemoveItemAsync(id, itemId);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return NoContent();
        }

        /// <summary>
        /// Method responsible for action: UndoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}/items/{itemId:guid}/undo")]
        [Authorize("outputs:undo-item")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<ActionResult> UndoItem(Guid id, Guid itemId)
        {
            ApplicationDataResult<OutputItemDto> result = await _applicationService.UndoItemAsync(id, itemId);

            if (!result.IsSuccess)
                return BadRequest(FactoryBadRequest(result));

            return NoContent();
        }
    }
}
