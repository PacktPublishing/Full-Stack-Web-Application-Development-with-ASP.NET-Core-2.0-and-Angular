using Macaria.Core.Models;
using System;
using Xunit;

namespace UnitTests.Core
{
    public class UserTests
    {
        [Fact]
        public void ShouldHaveSaltByDefault() {
            var user = new User();
            Assert.NotEqual(default(Byte[]),user.Salt);
        }
    }
}
