using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Macaria.API.Services;
using MediatR;
using Macaria.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using Macaria.Infrastructure.Middleware;
using Macaria.Infrastructure.Behaviours;
using System.Globalization;
using System.Collections.Generic;

namespace Macaria.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-CA");
                 
                options.SupportedCultures = new List<CultureInfo> {
                    new CultureInfo("en-CA"),
                    new CultureInfo("fr-CA")
                };
            });

            services.AddCustomConfiguration(Configuration);
            services.AddSecurity(Configuration);            
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            ConfigureDataStore(services);
            services.AddCustomSwagger();
            services.AddMediatR(typeof(Startup));
            services.AddCustomCache();
            services.AddSignalR();
            services.AddCustomMvc();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public virtual void ConfigureDataStore(IServiceCollection services)
        {
            services.AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"]);
        }

        public virtual void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseRequestLocalization();

            app.UseCors("CorsPolicy");
            ConfigureAuth(app);
            ConfigureTenantIdAndUsernameResolution(app);            
            app.UseMvc();
            app.UseCustomSwagger();
        }

        public virtual void ConfigureTenantIdAndUsernameResolution(IApplicationBuilder app)
        {
            app.UseMiddleware<TenantIdAndUsernameMiddleware>();
        }
    }
}
