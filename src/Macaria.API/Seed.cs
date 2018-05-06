using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Macaria.Infrastructure.Extensions;
using Macaria.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Macaria.API
{
    public class ApiConfiguration
    {
        public static void Seed(MacariaContext context)
        {
            UserConfiguration.Seed(context);
            TagConfiguration.Seed(context);

            context.SaveChanges();
        }
    }

    public class UserConfiguration
    {
        public static void Seed(MacariaContext context)
        {
            if (context.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Username == "quinntynebrown@gmail.com") == null)
            {
                var user = new User()
                {
                    Username = "quinntynebrown@gmail.com"
                };
                user.Password = new PasswordHasher().HashPassword(user,"P@ssw0rd");

                context.Users.Add(user);

            }

            context.SaveChanges();
        }
    }

    public class TagConfiguration
    {
        public static void Seed(MacariaContext context)
        {
            if (context.Tags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "Angular") == null)
                context.Tags.Add(new Tag() { Name = "Angular", Slug = "Angular".GenerateSlug() });

            if (context.Tags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "ASP.NET Core") == null)
                context.Tags.Add(new Tag() { Name = "ASP.NET Core", Slug = "ASP.NET Core".GenerateSlug() });

            context.SaveChanges();
        }
    }
}
