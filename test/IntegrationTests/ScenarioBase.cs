using Macaria.API.Seed;
using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using TestUtilities;

namespace IntegrationTests
{
    public class ScenarioBase
    {
        protected TestServer CreateServer()
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

            ResetDatabase(testServer.Host);

            return testServer;
        }

        protected void ResetDatabase(IWebHost host)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MacariaContext>();

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                context.Database.EnsureDeleted();

                context.Database.Migrate();

                ApiConfiguration.Seed(context, configuration);
            }
        }

        protected HubConnection GetHubConnection(HttpMessageHandler httpMessageHandler) {
            return new HubConnectionBuilder()
                            .WithUrl($"http://integrationtests/hub?token={TokenFactory.Get("quinntynebrown@gmail.com")}")
                            .WithMessageHandler((h) => httpMessageHandler)
                            .WithTransport(TransportType.LongPolling)
                            .Build();
        }
    }
}
