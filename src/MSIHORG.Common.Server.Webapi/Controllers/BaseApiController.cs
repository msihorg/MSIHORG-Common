// BlazorTemplate.API\Controllers\BaseApiController.cs

using Microsoft.AspNetCore.Mvc;

namespace MSIHORG.Common.Server.Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected ActionResult<T> HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }
    }
}
