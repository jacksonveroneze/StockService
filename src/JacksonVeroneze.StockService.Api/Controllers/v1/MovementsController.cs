using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Domain.Filters;
using JacksonVeroneze.StockService.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers.v1
{
    /// <summary>
    /// Class responsible for controller
    /// </summary>
    public class MovementsController : Controller
    {
        private readonly IMovementApplicationService _applicationService;

        /// <summary>
        /// Method responsible for initialize controller.
        /// </summary>
        /// <param name="applicationService"></param>
        public MovementsController(IMovementApplicationService applicationService)
            => _applicationService = applicationService;

        /// <summary>
        /// Method responsible for action: Filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("movements:filter")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IList<MovementModel>>> Filter(
            [FromQuery] MovementFilter filter)
            => Ok(await _applicationService.FilterAsync(filter));

        /// <summary>
        /// Method responsible for action: Find.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{productId:guid}")]
        [Authorize("movements:find")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<MovementModel>> Find(Guid productId)
        {
            MovementModel movementModel = await _applicationService.FindByProductAsync(productId);

            if (movementModel is null)
                return NoContent();

            return Ok(movementModel);
        }
    }
}
