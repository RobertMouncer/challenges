using Microsoft.AspNetCore.Mvc;

namespace challenges.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok();
        }
    }
}