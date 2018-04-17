using Macaria.Infrastructure.Middleware;
using Macaria.Infrastructure.OperationFilters;
using Microsoft.AspNetCore.Builder;

namespace Macaria.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Macaria API");
                
            });
            return app;
        }
    }
}
