using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok("Healthy");
        }
    }
}
