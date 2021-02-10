using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO;
using JacksonVeroneze.StockService.Application.DTO.Product;
using JacksonVeroneze.StockService.Application.Interfaces;
using JacksonVeroneze.StockService.Application.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductApplicationService _applicationService;

        public ProductsController(ILogger<ProductsController> logger, IProductApplicationService applicationService)
        {
            _logger = logger;
            _applicationService = applicationService;
        }

        //
        // Summary:
        //     /// Method responsible for action: Get. ///
        //
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> Get()
        {
            _logger.LogInformation("Request: Controller: {0} - Method: {1}",
                $"{nameof(ProductsController)}", $"{nameof(Get)}");

            return Ok(await _applicationService.FindAllAsync());
        }

        //
        // Summary:
        //     /// Method responsible for action: Get. ///
        //
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Find))]
        public async Task<ActionResult<ProductResultDto>> Find(Guid id)
        {
            _logger.LogInformation("Request: Controller: {0} - Method: {1} - Id: {2}",
                $"{nameof(ProductsController)}", $"{nameof(Find)}", id.ToString());

            return Ok(await _applicationService.FindAsync(id));
        }

        //
        // Summary:
        //     /// Method responsible for action: Add. ///
        //
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<ProductResultDto>> Add([FromBody] ProductRequestDto productRequestDto)
        {
            _logger.LogInformation("Request: Controller: {0} - Method: {1} - Data: {2}",
                $"{nameof(ProductsController)}", $"{nameof(Add)}", productRequestDto.ToString());

            ApplicationDataResult<ProductResultDto> result = await _applicationService.AddASync(productRequestDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(productRequestDto);
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
            _logger.LogInformation("Request: Controller: {0} - Method: {1} - Id: {2}",
                $"{nameof(ProductsController)}", $"{nameof(Delete)}", id.ToString());

            await _applicationService.RemoveASync(id);

            return NoContent();
        }
    }
}
