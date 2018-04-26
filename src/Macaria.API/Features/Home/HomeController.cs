using Microsoft.AspNetCore.Mvc;

namespace Macaria.API.Features.Home
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class HomeController
    {
        [HttpGet]
        public IActionResult Index()
            => new RedirectResult("~/swagger");
    }
}