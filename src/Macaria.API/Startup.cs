using Macaria.API.Behaviors;
using Macaria.API.Hubs;
using Macaria.Core.Behaviours;
using Macaria.Core.Extensions;
using Macaria.Core.Identity;
using Macaria.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Convert;

namespace Macaria.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc();
            services.AddCustomSecurity(Configuration);
            services.AddCustomSignalR();                        
            services.AddCustomSwagger();
            services.AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"]);            
            services.AddMediatR(typeof(Startup));                        
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EntityChangedNotificationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));            
        }
        
        public void Configure(IApplicationBuilder app)
        {
            if (ToBoolean(Configuration["isTest"]))
                app.UseMiddleware<AutoAuthenticationMiddleware>();
                    
            app.UseAuthentication();            
            app.UseCors("CorsPolicy");            
            app.UseMvc();
            app.UseSignalR(routes => routes.MapHub<AppHub>("/hub"));
            app.UseSwagger();
            app.UseSwaggerUI(options 
                => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Macaria API"));
        }        
    }
}
