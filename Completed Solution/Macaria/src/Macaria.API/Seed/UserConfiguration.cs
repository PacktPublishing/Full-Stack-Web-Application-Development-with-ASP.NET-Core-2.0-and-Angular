using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Macaria.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Macaria.API.Seed
{
    public class UserConfiguration
    {
        public static void Seed(MacariaContext context, Tenant tenant, IConfiguration configuration)
        {
            if (context.Users.IgnoreQueryFilters().FirstOrDefault(x => x.Username == "quinntynebrown@gmail.com") == null)
                context.Users.Add(new User() {
                    Username = "quinntynebrown@gmail.com",
                    Password = new EncryptionService(configuration).TransformPassword("P@ssw0rd"),
                    Tenant = tenant
                });

            context.SaveChanges();
        }
    }
}
