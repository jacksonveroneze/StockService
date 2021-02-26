using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JacksonVeroneze.StockService.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public abstract class Controller : ControllerBase
    {

    }
}
