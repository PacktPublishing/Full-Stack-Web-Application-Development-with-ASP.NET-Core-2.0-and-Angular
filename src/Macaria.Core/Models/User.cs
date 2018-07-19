using Macaria.Core.Common;
using System;
using System.Security.Cryptography;

namespace Macaria.Core.Models
{
    public class User: AggregateRoot
    {
        public User()
        {
            Salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(Salt);
            }
        }

        public Guid UserId { get; set; }
        = Guid.NewGuid();
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; private set; }
    }
}
