using Microsoft.Extensions.Configuration;
using System.IO;

namespace TestUtilities
{
    public class ConfigurationProvider
    {
        public static IConfigurationRoot Get()
            => new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(@"../../../../../src/Macaria.API/"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
    }
}
