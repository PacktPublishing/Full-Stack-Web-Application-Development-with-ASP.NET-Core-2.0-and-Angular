﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;

namespace IntegrationTests.Features.Tenants
{
    public class TenantScenarioBase
    {
        protected TestServer CreateServer() {
            var webHostBuilder = new WebHostBuilder()
                    .UseStartup(typeof(IntegrationTestsStartup))
                    .UseKestrel()
                    .UseConfiguration(TestUtilities.ConfigurationProvider.Get())
                    .ConfigureAppConfiguration((builderContext, config) =>
                    {
                        config
                        .AddJsonFile("settings.json");
                    });

            var testServer = new TestServer(webHostBuilder);

            return testServer;
        }

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
