﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Macaria.API.Behaviors;
using Macaria.API.Hubs;
using Macaria.Infrastructure.Behaviours;
using Macaria.Infrastructure.Extensions;
using Macaria.Infrastructure.Middleware;
using Macaria.Infrastructure;
using Macaria.Infrastructure.Services;

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
            services.AddHttpContextAccessor();

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new SignalRContractResolver()
            };

            var serializer = JsonSerializer.Create(settings);

            services.Add(new ServiceDescriptor(typeof(JsonSerializer),
                                               provider => serializer,
                                               ServiceLifetime.Transient));

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-CA");
                 
                options.SupportedCultures = new List<CultureInfo> {
                    new CultureInfo("en-CA"),
                    new CultureInfo("fr-CA")
                };
            });
            services.AddSignalR();
            services.AddCustomConfiguration(Configuration);
            services.AddSecurity(Configuration);            
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            ConfigureDataStore(services);
            services.AddCustomSwagger();
            services.AddMediatR(typeof(Startup));
            services.AddSignalR();
            services.AddCustomMvc();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EntityChangedNotificationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
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
            app.UseSignalR(routes =>
            {
                routes.MapHub<AppHub>("/hub");
            });
            app.UseCustomSwagger();
        }

        public virtual void ConfigureTenantIdAndUsernameResolution(IApplicationBuilder app)
        {
            app.UseMiddleware<TenantIdAndUsernameMiddleware>();
        }
    }
}