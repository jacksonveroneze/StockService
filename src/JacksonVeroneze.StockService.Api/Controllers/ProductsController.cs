using System.Net.Mime;
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
        public ActionResult Add()
        {
            _logger.LogInformation("Request: {0}", $"{nameof(ProductsController)} - {nameof(Add)}");

            return Ok("ok");
        }
    }
}
