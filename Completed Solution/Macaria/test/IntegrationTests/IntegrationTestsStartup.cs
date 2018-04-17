using IntegrationTests.Middleware;
using Macaria.API;
using Microsoft.AspNetCore.Builder;
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
            if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
            {
                app.UseMiddleware<TenantIdAndUsernameMiddleware>();
            }
            else
            {
                base.ConfigureTenantIdAndUsernameResolution(app);
            }
        }

        public override void ConfigureDataStore(IServiceCollection services)
        {
            if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
            {
                //TODO: InMemoryDataStore
                base.ConfigureDataStore(services);
            }
            else
            {
                base.ConfigureDataStore(services);
            }
            
        }
    }
}
