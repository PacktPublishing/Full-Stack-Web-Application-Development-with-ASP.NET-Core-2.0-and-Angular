using Macaria.API.Features.Users;
using Macaria.Core.Identity;
using Macaria.Core.Models;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API
{
    public class UserUnitTests
    {
        protected readonly Mock<IPasswordHasher> _passwordHasherMock;
        protected readonly Mock<ISecurityTokenFactory> _securityTokenFactory;

        public UserUnitTests()
        {
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _securityTokenFactory = new Mock<ISecurityTokenFactory>();

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<byte[]>(), "password"))
                .Returns("password");

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<byte[]>(),"changePassword"))
                .Returns("passwordChanged");

            _securityTokenFactory.Setup(x => x.Create("quinntynebrown@gmail.com")).Returns("token");
        }

        [Fact]
        public async Task ShouldHandleAuthenticateUserCommandRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleAuthenticateUserCommandRequest")
                .Options;

            using (var context = new AppDbContext(options))
            {                
                context.Users.Add(new User()
                {
                    Username = "quinntynebrown@gmail.com",
                    Password = "password"
                });

                context.SaveChanges();

                var handler = new AuthenticateCommand.Handler(context, _securityTokenFactory.Object, _passwordHasherMock.Object);

                var response = await handler.Handle(new AuthenticateCommand.Request()
                {
                    Username = "quinntynebrown@gmail.com",
                    Password = "password"
                }, default(CancellationToken));

                Assert.Equal("token", response.AccessToken);
            }
        }        
    }
}
