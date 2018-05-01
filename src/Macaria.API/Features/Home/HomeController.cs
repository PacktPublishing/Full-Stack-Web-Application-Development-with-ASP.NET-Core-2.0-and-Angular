using Microsoft.AspNetCore.Mvc;

namespace Macaria.API.Features.Home
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class HomeController
    {
        [HttpGet("health")]
        public IActionResult Health() {
            return new OkObjectResult(new {
                Status = "Healthy"
            });
        }
            

        [HttpGet]
        public IActionResult Index()
            => new RedirectResult("~/swagger");
    }
}