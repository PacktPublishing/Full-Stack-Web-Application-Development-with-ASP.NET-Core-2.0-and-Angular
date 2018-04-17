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
                        Username = "quinntynebrown@gmail.com",
                        Password = "P@ssw0rd"
                    });

                Assert.True(response.UserId != default(int));
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

                Assert.True(response.UserId != default(int));
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
                        UserId = 13,
                        Username = "quinntynebrown@gmail.com",
                    });

                Assert.True(response.UserId != default(int));
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetUsersQuery.Response>(Get.Users);

                Assert.True(response.Users.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetUserByIdQuery.Response>(Get.UserById(13));

                Assert.True(response.User.UserId != default(int));
            }
        }
                
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.User(13));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
