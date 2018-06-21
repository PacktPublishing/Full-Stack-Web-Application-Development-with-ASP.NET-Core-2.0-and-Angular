using FluentValidation;
using Macaria.API.Features;
using Macaria.API.Features.Notes;
using Macaria.API.Features.Tags;
using Macaria.API.Features.Users;
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
                .AddCustomSecurity(Configuration)
                .AddCustomSignalR()
                .AddCustomSwagger()
                .AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"],Configuration.GetValue<bool>("isTest"))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddTransient<IValidator<AuthenticateCommand.Request>, AuthenticateCommand.Validator>()
                .AddTransient<IValidator<SaveNoteCommand.Request>, SaveNoteCommand.Validator>()
                .AddTransient<IValidator<RemoveNoteCommand.Request>, RemoveNoteCommand.Validator>()
                .AddTransient<IValidator<SaveTagCommand.Request>, SaveTagCommand.Validator>()
                .AddTransient<IValidator<RemoveTagCommand.Request>, RemoveTagCommand.Validator>()
                .AddMediatR(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app)
        {
            if(Configuration.GetValue<bool>("isTest"))
                app.UseMiddleware<AutoAuthenticationMiddleware>();
                    
            app.UseAuthentication()            
                .UseCors("CorsPolicy")            
                .UseMvc()
                .UseSignalR(routes => routes.MapHub<IntegrationEventsHub>("/hub"))
                .UseSwagger()
                .UseSwaggerUI(options 
                => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Macaria API"));
        }        
    }
}
