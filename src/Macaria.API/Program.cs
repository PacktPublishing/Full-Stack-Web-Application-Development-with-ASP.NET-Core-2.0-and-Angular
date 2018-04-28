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
                if (args.Contains("ci"))
                    args = new string[4] { "dropdb", "migratedb", "seeddb", "stop" };

                if (args.Contains("dropdb"))
                    GetMacariaContext(scope).Database.EnsureDeleted();

                if (args.Contains("migratedb"))
                    GetMacariaContext(scope).Database.Migrate();

                if (args.Contains("seeddb"))
                {
                    GetMacariaContext(scope).Database.EnsureCreated();
                    ApiConfiguration.Seed(GetMacariaContext(scope), GetConfiguration(scope));            
                }
                
                if (args.Contains("stop"))
                    Environment.Exit(0);
            }
        }
        private static MacariaContext GetMacariaContext(IServiceScope services)
            => services.ServiceProvider.GetRequiredService<MacariaContext>();

        private static IConfiguration GetConfiguration(IServiceScope services)
            => services.ServiceProvider.GetRequiredService<IConfiguration>(); 
    }
}
