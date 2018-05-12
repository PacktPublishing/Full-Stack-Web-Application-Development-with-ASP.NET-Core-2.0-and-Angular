using Macaria.Core.Interfaces;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Macaria.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {                
        public static IServiceCollection AddDataStore(this IServiceCollection services,
                                               string connectionString)
        {
            services.AddScoped<IMacariaContext, MacariaContext>();

            services.AddDbContext<MacariaContext>(options =>
            {                
                options
                .UseLoggerFactory(MacariaContext.ConsoleLoggerFactory)
                .UseSqlServer(connectionString, b=> b.MigrationsAssembly("Macaria.Infrastructure"));
            });

            return services;
        }
    }
}
