using Macaria.API;
using Macaria.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class IntegrationTestServer : TestServer
    {
        public IntegrationTestServer(IWebHostBuilder webHostBuilder)
            : base(webHostBuilder) { }

        public void ResetDatabase()
        {
            var services = (IServiceScopeFactory)Host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.EnsureDeleted();

                context.Database.EnsureCreated();

                SeedData.Seed(context);
            }
        }

        public void DropDatabase()
        {
            var services = (IServiceScopeFactory)Host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.EnsureDeleted();
            }
        }

        public new void Dispose()
        {
            DropDatabase();

            base.Dispose();
        }

    }
}
