using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IntegrationTests.Middleware
{
    public class TenantIdAndUsernameMiddleware
    {
        private readonly RequestDelegate _next;
        public TenantIdAndUsernameMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Items.Add("TenantId", "60DE04D9-E441-E811-9D3A-D481D7227E7A");
            httpContext.Items.Add("Username", "quinntynebrown@gmail.com");
            await _next.Invoke(httpContext);
        }
    }
}