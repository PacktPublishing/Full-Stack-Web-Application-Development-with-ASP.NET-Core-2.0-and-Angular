using Macaria.API.Seed;
using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IntegrationTests
{
    public class ScenarioBase
    {
        protected TestServer CreateServer(Action<MacariaContext> setUpData = null)
        {
            var webHostBuilder = new WebHostBuilder()
                    .UseStartup(typeof(IntegrationTestsStartup))
                    .UseKestrel()
                    .UseConfiguration(TestUtilities.ConfigurationProvider.Get())
                    .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        config
                        .AddJsonFile("settings.json");
                    });

            var testServer = new TestServer(webHostBuilder);

            ResetDatabase(testServer.Host, setUpData);

            return testServer;
        }

        protected void ResetDatabase(IWebHost host, Action<MacariaContext> setUpData = null)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MacariaContext>();

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                context.Database.EnsureDeleted();

                context.Database.Migrate();

                ApiConfiguration.Seed(context, configuration);

                setUpData?.Invoke(context);
            }
        }
    }
}
