namespace Macaria.Infrastructure.Configuration
{
    public class AuthenticationSettings
    {
        public string TokenPath { get; set; }
        public int ExpirationMinutes { get; set; }
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public string AuthType { get; set; }
    }
}
