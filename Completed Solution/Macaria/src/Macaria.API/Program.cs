using Macaria.Infrastructure.Data;
using Macaria.API.Seed;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Macaria.Core.Entities;
using Microsoft.Extensions.Configuration;

namespace Macaria.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder().Build();

            ProcessDbCommands(args, host);

            host.Run();
        }
        
        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();

        private static void ProcessDbCommands(string[] args, IWebHost host)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));
            using (var scope = services.CreateScope())
            {
                if (args.Contains("dropdb"))
                {
                    GetMacariaContext(scope).Database.EnsureDeleted();
                }

                if (args.Contains("seeddb"))
                {
                    GetMacariaContext(scope).Database.EnsureCreated();
                    SeedContext(GetMacariaContext(scope), GetConfiguration(scope));            
                }

                if (args.Contains("migratedb"))
                {
                    GetMacariaContext(scope).Database.Migrate();
                }
                
                if (args.Contains("stop"))
                {
                    Environment.Exit(0);
                }
            }
        }
        private static MacariaContext GetMacariaContext(IServiceScope services)
            => services.ServiceProvider.GetRequiredService<MacariaContext>();

        private static IConfiguration GetConfiguration(IServiceScope services)
            => services.ServiceProvider.GetRequiredService<IConfiguration>();

        public static void SeedContext(MacariaContext context, IConfiguration configuration) {
            var tenant = context.Tenants.IgnoreQueryFilters().SingleOrDefault(x => x.Name == "Default");

            if (tenant == null) { context.Tenants.Add(tenant = new Tenant() { Name = "Default", TenantId = new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A") }); }

            context.SaveChanges();

            UserConfiguration.Seed(context,tenant,configuration);
            TagConfiguration.Seed(context,tenant);
        }  
    }
}
