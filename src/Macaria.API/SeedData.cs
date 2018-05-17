using Macaria.Core.Entities;
using Macaria.Core.Extensions;
using Macaria.Core.Identity;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Macaria.API
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            UserConfiguration.Seed(context);
            TagConfiguration.Seed(context);

            context.SaveChanges();
        }

        internal class UserConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Username == "quinntynebrown@gmail.com") == null)
                {
                    var user = new User()
                    {
                        Username = "quinntynebrown@gmail.com"
                    };
                    user.Password = new PasswordHasher().HashPassword(user.Salt, "P@ssw0rd");

                    context.Users.Add(user);

                }

                context.SaveChanges();
            }
        }

        internal class TagConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.Tags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "Angular") == null)
                    context.Tags.Add(new Tag() { Name = "Angular", Slug = "Angular".GenerateSlug() });

                if (context.Tags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "ASP.NET Core") == null)
                    context.Tags.Add(new Tag() { Name = "ASP.NET Core", Slug = "ASP.NET Core".GenerateSlug() });

                context.SaveChanges();
            }
        }
    }
}
