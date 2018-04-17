using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;

namespace IntegrationTests.Features.Notes
{
    public class NoteScenarioBase
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
            public static string Notes = "api/notes";

            public static string NoteById(int id)
            {
                return $"api/notes/{id}";
            }
        }

        public static class Post
        {
            public static string Notes = "api/notes";

            public static string AddTag(int noteId, int tagId)
            {
                return $"api/notes/{noteId}/tag/{tagId}";
            }
        }

        public static class Delete
        {
            public static string Note(int id)
                => $"api/notes/{id}";
            
            public static string RemoveTag(int noteId, int tagId)
                => $"api/notes/{noteId}/tag/{tagId}";            
        }

    }
}
