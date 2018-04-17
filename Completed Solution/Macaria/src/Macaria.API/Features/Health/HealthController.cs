using Microsoft.AspNetCore.Mvc;

namespace Macaria.API.Features.Health
{
    [ApiController]
    [Route("api/health")]
    public class HealthController
    {
        [HttpGet("get")]
        public ActionResult Get() => new OkResult();
    }
}
