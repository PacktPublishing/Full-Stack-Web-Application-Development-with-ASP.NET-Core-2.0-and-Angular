using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Macaria.Infrastructure.Services
{
    public interface IEncryptionService
    {
        string TransformPassword(string password);
        string EncryptString(string plainText);
        string DecryptString(string cipherText);
        string EncryptUri(string plainText);
    }

    public class EncryptionService : IEncryptionService
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _salt = Encoding.ASCII.GetBytes("42kbf43w7i8kku234cx56jymj567o560213");
        private string sharedSecret { get { return _configuration["Authentication:JwtKey"]; } }

        public EncryptionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string TransformPassword(string password)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] Hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(Hash);
        }

        public string EncryptString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");

            string outStr = null;
            RijndaelManaged aesAlg = null;

            try
            {

                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return outStr;
        }

        public string DecryptString(string cipherText)
        {

            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            RijndaelManaged aesAlg = null;


            string plaintext = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);


                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        public string EncryptUri(string plainText)
        {
            return HttpUtility.UrlEncode(EncryptString(plainText));
        }
    }
}
