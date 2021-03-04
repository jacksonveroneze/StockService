using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class Controller : ControllerBase
    {

    }
}
