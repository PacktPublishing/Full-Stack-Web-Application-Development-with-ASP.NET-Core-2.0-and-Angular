using Macaria.API;
using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class IntegrationTestsStartup: Startup
    {
        public IntegrationTestsStartup(IConfiguration configuration) : base(configuration)
        {

        }

        public override void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseMiddleware<AutoAuthorizeMiddleware>();
        }

        public override void ConfigureDataStore(IServiceCollection services)
        {
            services.AddScoped<IMacariaContext, MacariaContext>();

            services.AddDbContext<MacariaContext>(options =>
            {
                options.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=MacariaIntegrationTests;Integrated Security=SSPI;", b => b.MigrationsAssembly("Macaria.Infrastructure"));
            });
        }
    }

    public class AutoAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;
        public AutoAuthorizeMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            var identity = new ClaimsIdentity("Macaria");
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, "quinntynebrown@gmail.com"));
            httpContext.User.AddIdentity(identity);
            await _next.Invoke(httpContext);
        }
    }
}
