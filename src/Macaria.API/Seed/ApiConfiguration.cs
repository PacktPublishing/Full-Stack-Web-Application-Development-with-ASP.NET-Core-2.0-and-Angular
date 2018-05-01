using Macaria.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Macaria.API.Seed
{
    public class ApiConfiguration
    {
        public static void Seed(MacariaContext context, IConfiguration configuration)
        {
            UserConfiguration.Seed(context, configuration);
            TagConfiguration.Seed(context);

            context.SaveChanges();
        }
    }
}
