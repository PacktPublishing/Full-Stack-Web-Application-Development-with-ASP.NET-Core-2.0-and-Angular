using Macaria.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;

namespace TestUtilities
{
    public static class TokenFactory
    {
        private static readonly IConfiguration _configuration = ConfigurationProvider.Get();

        private static readonly TokenProvider _tokenProvider = new TokenProvider(_configuration);

        public static string Get(string username) {
            return _tokenProvider.Get(username);
        }
    }
}
