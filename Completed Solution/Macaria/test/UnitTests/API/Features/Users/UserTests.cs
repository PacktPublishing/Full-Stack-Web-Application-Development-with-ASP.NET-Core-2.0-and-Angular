using Macaria.API.Features.Users;
using Macaria.Infrastructure.Data;
using Macaria.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API.Features.Users
{    
    public class UserTests
    {
        private readonly Mock<IEncryptionService> _encryptionServiceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<HttpContext> _httpContextMock;

        public UserTests()
        {
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextMock = new Mock<HttpContext>();

            _httpContextMock.Setup(x => x.Items).Returns(new Dictionary<object, object>
            {
                { "TenantId", $"{new Guid("60DE04D9-E441-E811-9D3A-D481D7227E7A")}" },
                { "Username", "quinntynebrown@gmail.com" }
            });

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(_httpContextMock.Object);
        }

        [Fact]
        public async Task ShouldCreateUser() {

            _encryptionServiceMock.Setup(x => x.TransformPassword("password"))
                .Returns("password");

            var options = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldCreateUser")
                .Options;

            using (var context = new MacariaContext(options,_httpContextAccessorMock.Object))
            {
                var cancellationToken = default(CancellationToken);

                var handler = new CreateUserCommand.Handler(context, _encryptionServiceMock.Object);

                var response = await handler.Handle(new CreateUserCommand.Request() {
                    Username = "quinntynebrown@gmail.com",
                    Password ="password"
                }, cancellationToken);

                Assert.Equal(1, response.UserId);

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }


            var options1 = new DbContextOptionsBuilder<MacariaContext>()
                .UseInMemoryDatabase(databaseName: "ShouldCreateUser1")
                .Options;

            using (var context = new MacariaContext(options1, _httpContextAccessorMock.Object))
            {
                var cancellationToken = default(CancellationToken);

                var handler = new CreateUserCommand.Handler(context, _encryptionServiceMock.Object);

                var response = await handler.Handle(new CreateUserCommand.Request()
                {
                    Username = "quinntynebrown@gmail.com",
                    Password = "password"
                }, cancellationToken);

                Assert.Equal(1, response.UserId);
            }
        }
    }
}
