using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Macaria.API.Seed
{
    public class TagConfiguration
    {
        public static void Seed(MacariaContext context, Tenant tenant)
        {
            if (context.Tags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "Angular") == null)
                context.Tags.Add(new Tag() { Name = "Angular", Tenant = tenant });

            if (context.Tags.IgnoreQueryFilters().FirstOrDefault(x => x.Name == "ASP.NET Core") == null)
                context.Tags.Add(new Tag() { Name = "ASP.NET Core", Tenant = tenant });

            context.SaveChanges();
        }
    }
}
