using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IntegrationTests.Middleware
{
    public class AutoAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
        public AutoAuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var identity = new ClaimsIdentity("Macaria");
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, "quinntynebrown@gmail.com"));
            httpContext.User.AddIdentity(identity);
            await _next.Invoke(httpContext);
        }
    }
}