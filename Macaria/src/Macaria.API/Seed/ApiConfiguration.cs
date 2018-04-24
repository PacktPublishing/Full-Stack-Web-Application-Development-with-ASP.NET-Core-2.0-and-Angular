using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Macaria.API.Seed
{
    public class ApiConfiguration
    {
        public static void Seed(MacariaContext context, IConfiguration configuration)
        {
            var tenant = context.Tenants.IgnoreQueryFilters().SingleOrDefault(x => x.Name == "Default");

            if (tenant == null) { context.Tenants.Add(tenant = new Tenant() { Name = "Default", TenantId = new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A") }); }

            context.SaveChanges();

            UserConfiguration.Seed(context, tenant, configuration);
            TagConfiguration.Seed(context, tenant);

            context.SaveChanges();
        }
    }
}
