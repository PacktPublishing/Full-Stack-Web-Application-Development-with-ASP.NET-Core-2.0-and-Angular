using Macaria.Core.Extensions;
using Macaria.Core.Identity;
using Macaria.Core.Models;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Macaria.Infrastructure.Extensions
{
    public static class ServiceScopeExtensions
    {
        public static void SeedData(this IServiceScope scope) {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            if (context.Users.FirstOrDefault(x => x.Username == "quinntynebrown@gmail.com") == null)
            {
                var user = new User()
                {
                    Username = "quinntynebrown@gmail.com"
                };
                user.Password = new PasswordHasher().HashPassword(user.Salt, "P@ssw0rd");
                
                context.Users.Add(user);
            }

            if (context.Tags.FirstOrDefault(x => x.Name == "Angular") == null)
                context.Tags.Add(new Tag() { Name = "Angular", Slug = "Angular".ToSlug() });

            if (context.Tags.FirstOrDefault(x => x.Name == "ASP.NET Core") == null)
                context.Tags.Add(new Tag() { Name = "ASP.NET Core", Slug = "ASP.NET Core".ToSlug() });

        }

        public static void DropDataBase(this IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureDeleted();
        }

        public static void MigrateDatabase(this IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.Migrate();
        }
    }
}
