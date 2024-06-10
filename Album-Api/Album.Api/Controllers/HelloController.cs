using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Album.Api.Services;
using Album.Api.Models;

namespace Album.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly IGreetingService _greetingService;
        private readonly ILogger<HelloController> _logger;

        public HelloController(IGreetingService greetingService, ILogger<HelloController> logger)
        {
            _greetingService = greetingService;
            _logger = logger;
        }

        [HttpGet("hello")]
        public ActionResult<GreetingResponse> GetHello([FromQuery] string name)
        {
            _logger.LogInformation("Received request for hello with name: {Name}", name);
            var message = _greetingService.GetGreeting(name);
            return Ok(new GreetingResponse { Message = message });
        }
    }
}
