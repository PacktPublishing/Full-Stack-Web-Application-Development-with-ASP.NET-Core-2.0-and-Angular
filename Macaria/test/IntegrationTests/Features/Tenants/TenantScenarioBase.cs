using System;

namespace IntegrationTests.Features.Tenants
{
    public class TenantScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Tenants = "api/tenants";

            public static string TenantById(Guid id)
            {
                return $"api/tenants/{id}";
            }
        }

        public static class Post
        {
            public static string Tenants = "api/tenants";
            public static string Verify(Guid id)
            {
                return $"api/tenants/{id}/verify";
            }
        }

        public static class Delete
        {
            public static string Tenant(Guid id)
            {
                return $"api/tenants/{id}";
            }
        }
    }
}
