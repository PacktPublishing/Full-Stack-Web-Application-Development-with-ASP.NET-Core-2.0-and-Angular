using Macaria.API.Features.Users;
using System.Linq;
using System.Threading.Tasks;
using Macaria.Infrastructure.Extensions;
using Xunit;

namespace IntegrationTests.Features.Users
{
    public class UserScenarios: UserScenarioBase
    {
        [Fact]
        public async Task ShouldCreateUser()
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
        public async Task ShouldChangeUserPassword()
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
        public async Task ShouldAuthenticateUser()
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
        public async Task ShouldUpdateUser()
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
        public async Task ShouldGetAllUsers()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetUsersQuery.Response>(Get.Users);

                Assert.True(response.Users.Count() == 1);
            }
        }


        [Fact]
        public async Task ShouldGetUserById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetUserByIdQuery.Response>(Get.UserById(1));

                Assert.True(response.User.UserId == 1);
            }
        }
                
        [Fact]
        public async Task ShouldDeleteUser()
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
