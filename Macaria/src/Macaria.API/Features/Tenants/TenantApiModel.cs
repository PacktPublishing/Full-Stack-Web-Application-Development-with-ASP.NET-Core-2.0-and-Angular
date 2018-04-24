using Macaria.Core.Entities;
using System;

namespace Macaria.API.Features.Tenants
{
    public class TenantApiModel
    {        
        public Guid TenantId { get; set; }
        public string Name { get; set; }

        public static TenantApiModel FromTenant(Tenant tenant)
            => new TenantApiModel
            {
                TenantId = tenant.TenantId,
                Name = tenant.Name
            };
    }
}
