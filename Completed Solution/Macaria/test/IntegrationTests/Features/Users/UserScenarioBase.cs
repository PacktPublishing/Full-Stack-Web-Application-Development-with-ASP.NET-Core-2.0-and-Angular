using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;

namespace IntegrationTests.Features.Users
{
    public class UserScenarioBase
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
            public static string Users = "api/users";

            public static string UserById(int id)
            {
                return $"api/users/{id}";
            }
        }

        public static class Post
        {
            public static string Users = "api/users";
            public static string Token = "api/users/token";
        }

        public static class Put
        {
            public static string Users = "api/users";
        }

        public static class Delete
        {
            public static string User(int id)
            {
                return $"api/users/{id}";
            }
        }

    }
}
