using IntegrationTests.Middleware;
using Macaria.API;
using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class IntegrationTestsStartup: Startup
    {
        public IntegrationTestsStartup(IConfiguration configuration) : base(configuration)
        {

        }
        public override void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
            {
                
                app.UseMiddleware<AutoAuthorizeMiddleware>();
            }
            else
            {
                base.ConfigureAuth(app);
            }
        }

        public override void ConfigureTenantIdAndUsernameResolution(IApplicationBuilder app)
        {
            app.UseMiddleware<TenantIdAndUsernameMiddleware>();
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
}
