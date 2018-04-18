using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Macaria.API.Features.Health
{
    [ApiController]
    [Route("api/health")]
    public class HealthController
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public HealthController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("get")]
        public ActionResult Get() {

            var rqf = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            // Culture contains the information of the requested culture
            var culture = rqf.RequestCulture.Culture;

            return new OkResult();
        } 
    }
}
