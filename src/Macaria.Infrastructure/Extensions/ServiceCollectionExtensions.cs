using Macaria.Core.Interfaces;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Macaria.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {                
        public static IServiceCollection AddDataStore(this IServiceCollection services,
                                               string connectionString, bool useInMemoryDatabase = false)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();

            return services.AddDbContext<AppDbContext>(options =>
            {
                options
                .UseLoggerFactory(AppDbContext.ConsoleLoggerFactory);

                _ = useInMemoryDatabase 
                ? options.UseInMemoryDatabase(databaseName: "Macaria")
                : options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Macaria.Infrastructure"));
            });          
        }
    }
}
