using FluentValidation.AspNetCore;
using Macaria.API.Features;
using Macaria.Core.Behaviours;
using Macaria.Core.Extensions;
using Macaria.Core.Identity;
using Macaria.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Macaria.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services
                .AddCustomSecurity(Configuration)
                .AddCustomSignalR()
                .AddCustomSwagger()
                .AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"],Configuration.GetValue<bool>("isTest"))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddMediatR(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app)
        {
            if(Configuration.GetValue<bool>("isTest"))
                app.UseMiddleware<AutoAuthenticationMiddleware>();
                    
            app.UseAuthentication()            
                .UseCors(CorsDefaults.Policy)            
                .UseMvc()
                .UseSignalR(routes => routes.MapHub<IntegrationEventsHub>("/hub"))
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Macaria API");
                    options.RoutePrefix = string.Empty;
                });
        }        
    }
}
