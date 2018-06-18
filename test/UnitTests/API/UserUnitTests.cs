using Macaria.API.Features.Users;
using Macaria.Core.Models;
using Macaria.Core.Identity;
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
        protected readonly Mock<ITokenProvider> _tokenProvider;

        public UserUnitTests()
        {
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _tokenProvider = new Mock<ITokenProvider>();

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<byte[]>(), "password"))
                .Returns("password");

            _passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<byte[]>(),"changePassword"))
                .Returns("passwordChanged");

            _tokenProvider.Setup(x => x.Get("quinntynebrown@gmail.com")).Returns("token");
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

                var handler = new AuthenticateCommand.Handler(context, _tokenProvider.Object, _passwordHasherMock.Object);

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
