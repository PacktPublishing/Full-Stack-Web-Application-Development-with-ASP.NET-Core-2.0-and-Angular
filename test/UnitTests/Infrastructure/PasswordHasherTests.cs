using Macaria.Infrastructure.Identity;
using Xunit;

namespace UnitTests.Infrastructure
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _passwordHasher;

        public PasswordHasherTests()
            => _passwordHasher = new PasswordHasher();

        [Fact]
        public void ShouldHashPasswordShouldBeAPureFunction() {

            var plainTextPassword = "P@ssw0rd";

            var hashedPassword1 = _passwordHasher.HashPassword(new byte[0] { }, plainTextPassword);

            var hashedPassword2 = _passwordHasher.HashPassword(new byte[0] { }, plainTextPassword);
            
            Assert.NotEqual(plainTextPassword, hashedPassword1);

            Assert.Equal(hashedPassword1, hashedPassword2);
        }
    }
}
