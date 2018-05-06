using Macaria.Core.Entities;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

//https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-2.1

namespace Macaria.Infrastructure.Identity
{
    public interface IPasswordHasher
    {
        string HashPassword(User user, string password);
    }

    public class PasswordHasher : IPasswordHasher
    {        
        public string HashPassword(User user, string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: user.Salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        }
    }
}
