using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(ILogger<PurchasesController> logger)
            => _logger = logger;

        //
        // Summary:
        //     /// Method responsible for action: Get (GET). ///
        //
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public ActionResult Get()
        {
            return Ok("ok");
        }
    }
}
