using Macaria.API.Features.Users;
using System.Linq;
using System.Threading.Tasks;
using TestUtilities.Extensions;
using Xunit;

namespace IntegrationTests.Features.Users
{
    public class UserScenarios: UserScenarioBase
    {
        [Fact]
        public async Task ShouldCreate()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<CreateUserCommand.Request, CreateUserCommand.Response>(Post.Users, new CreateUserCommand.Request()
                    {
                        Username = "Quinn",
                        Password = "Foo"
                    });

                Assert.True(response.UserId == 2);
            }
        }

        [Fact]
        public async Task ShouldChangePassword()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<UserChangePasswordCommand.Request, Task>(Post.ChangePassword, new UserChangePasswordCommand.Request()
                    {
                        UserId = 1,
                        ConfirmPassword = "Foo",
                        Password = "Foo"
                    });
            }
        }

        [Fact]
        public async Task ShouldAuthenticate()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<AuthenticateCommand.Request, AuthenticateCommand.Response>(Post.Token, new AuthenticateCommand.Request()
                    {
                        Username = "quinntynebrown@gmail.com",
                        Password = "P@ssw0rd"
                    });

                Assert.True(response.UserId == 1);
                Assert.True(response.AccessToken != default(string));
            }
        }

        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PutAsAsync<UpdateUserCommand.Request, UpdateUserCommand.Response>(Put.Users, new UpdateUserCommand.Request() {
                        UserId = 1,
                        Username = "quinntynebrown@gmail.com",
                    });

                Assert.True(response.UserId == 1);
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetUsersQuery.Response>(Get.Users);

                Assert.True(response.Users.Count() == 1);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetUserByIdQuery.Response>(Get.UserById(1));

                Assert.True(response.User.UserId == 1);
            }
        }
                
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.User(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
