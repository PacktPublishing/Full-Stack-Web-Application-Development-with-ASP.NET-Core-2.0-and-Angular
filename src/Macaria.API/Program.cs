using Macaria.Infrastructure.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
                    scope.DropDataBase();

                if (args.Contains("migratedb"))
                    scope.MigrateDatabase();

                if (args.Contains("seeddb"))
                    scope.SeedData();
                
                if (args.Contains("stop"))
                    Environment.Exit(0);
            }
        }        
    }
}
