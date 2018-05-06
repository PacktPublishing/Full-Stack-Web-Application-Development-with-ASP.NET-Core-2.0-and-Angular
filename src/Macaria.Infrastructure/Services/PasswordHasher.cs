using System;
using System.Security.Cryptography;
using System.Text;

namespace Macaria.Infrastructure.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }

    public class PasswordHasher : IPasswordHasher
    {        
        public string HashPassword(string password)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] Hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(Hash);
        }
    }
}
