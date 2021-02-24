using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO.Purchase;
using JacksonVeroneze.StockService.Application.DTO.PurchaseItem;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using JacksonVeroneze.StockService.Domain.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly ILogger<PurchasesController> _logger;
        private readonly IPurchaseApplicationService _applicationService;

        public PurchasesController(ILogger<PurchasesController> logger, IPurchaseApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        //
        // Summary:
        //     /// Method responsible for action: Filter. ///
        //
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> Filter([FromQuery] Pagination pagination, [FromQuery] PurchaseFilter filter)
            => Ok(await _applicationService.FilterAsync(pagination, filter));

        //
        // Summary:
        //     /// Method responsible for action: Find. ///
        //
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<PurchaseDto>> Find(Guid id)
            => Ok(await _applicationService.FindAsync(id));

        //
        // Summary:
        //     /// Method responsible for action: Add. ///
        //
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<PurchaseDto>> Add([FromBody] AddOrUpdatePurchaseDto purchaseDto)
        {
            ApplicationDataResult<PurchaseDto> result = await _applicationService.AddAsync(purchaseDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Find), new {id = result.Data.Id}, result.Data);
        }

        //
        // Summary:
        //     /// Method responsible for action: Delete. ///
        //
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _applicationService.RemoveAsync(id);

            return NoContent();
        }

        //
        // Summary:
        //     /// Method responsible for action: Close. ///
        //
        [HttpPut("{id}/close")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async Task<IActionResult> Close(Guid id)
        {
            await _applicationService.CloseAsync(id);

            return NoContent();
        }

        //
        // Summary:
        //     /// Method responsible for action: FindItems. ///
        //
        [HttpGet("{id}/items")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> FindItems(Guid id)
        {
            IList<PurchaseItemDto> result = await _applicationService.FindItensAsync(id);

            return Ok(result);
        }

        //
        // Summary:
        //     /// Method responsible for action: FindItem. ///
        //
        [HttpGet("{id}/items/{idItem}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> FindItem(Guid id, Guid idItem)
        {
            PurchaseItemDto result = await _applicationService.FindItemAsync(id, idItem);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        //
        // Summary:
        //     /// Method responsible for action: Add. ///
        //
        [HttpPost("{id}/items")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<PurchaseDto>> AddItem([FromBody] AddOrUpdatePurchaseItemDto purchaseItemDto)
        {
            ApplicationDataResult<PurchaseItemDto> result = await _applicationService.AddItemAsync(purchaseItemDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Find), new {id = result.Data.Id}, result.Data);
        }
    }
}
