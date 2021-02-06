using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using JacksonVeroneze.StockService.Application.DTO;
using JacksonVeroneze.StockService.Application.Interfaces;
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
        //     /// Method responsible for action: Add (POST). ///
        //
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<ProductDto>> Add([FromBody] ProductDto productDto)
        {
            _logger.LogInformation("Request: Controller: {0} - Method: {1} - Data: {2}",
                $"{nameof(ProductsController)}", $"{nameof(Add)}", productDto.ToString());

            ProductDto result = await _applicationService.AddASync(productDto);

            if (result is null)
                return BadRequest(_applicationService.ValidationResult.Errors.Select(x => x.ErrorMessage));

            return Ok(productDto);
        }
    }
}
