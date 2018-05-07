using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Macaria.API.Behaviors;
using Macaria.API.Hubs;
using Macaria.Infrastructure.Behaviours;
using Macaria.Infrastructure.Extensions;
using Macaria.Infrastructure.Identity;

namespace Macaria.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomSignalR();
            services.AddCustomConfiguration(Configuration);
            services.AddCustomSecurity(Configuration);                        
            services.AddCustomMvc();
            services.AddCustomSwagger();

            ConfigureDataStore(services);

            services.AddMediatR(typeof(Startup));                        
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EntityChangedNotificationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));            
        }

        public virtual void ConfigureDataStore(IServiceCollection services)
            => services.AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"]);

        public virtual void ConfigureAuth(IApplicationBuilder app)
            => app.UseAuthentication();

        public void Configure(IApplicationBuilder app)
        {            
            app.UseCors("CorsPolicy");
            ConfigureAuth(app);            
            app.UseMvc();
            app.UseSignalR(routes => routes.MapHub<AppHub>("/hub"));
            app.UseCustomSwagger();
        }
    }
}
