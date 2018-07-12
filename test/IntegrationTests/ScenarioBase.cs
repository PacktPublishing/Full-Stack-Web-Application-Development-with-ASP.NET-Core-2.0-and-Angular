using Macaria.API;
using Macaria.Infrastructure.Data;
using Macaria.Infrastructure.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace IntegrationTests
{
    public class ScenarioBase
    {
        protected TestServer CreateServer()
        {            
            var webHostBuilder = new WebHostBuilder()
                    .UseStartup(typeof(Startup))
                    .UseKestrel()
                    .UseConfiguration(GetConfiguration())
                    .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        config
                        .AddInMemoryCollection(new Dictionary<string, string>
                        {
                            { "isTest", "true"}
                        });
                    });

            var testServer = new TestServer(webHostBuilder);

            var services = (IServiceScopeFactory)testServer.Host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {                
                scope.SeedData();
            }

            return testServer;
        }

        protected HubConnection GetHubConnection(HttpMessageHandler httpMessageHandler) 
            => new HubConnectionBuilder()
                            .WithUrl($"http://integrationtests/hub",(options) => {
                                options.Transports = HttpTransportType.ServerSentEvents;
                                options.HttpMessageHandlerFactory = h => httpMessageHandler;
                            })
                            .ConfigureLogging(logging => logging.AddConsole())
                            .Build();

        protected IConfiguration GetConfiguration() => new ConfigurationBuilder()
                .AddUserSecrets(typeof(Startup).GetTypeInfo().Assembly)
                .Build();
    }    
}
