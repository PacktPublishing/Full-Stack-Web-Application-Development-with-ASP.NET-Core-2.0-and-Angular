using Macaria.API.Features.Tags;
using System.Linq;
using System.Threading.Tasks;
using TestUtilities.Extensions;
using Xunit;

namespace IntegrationTests.Features.Tags
{
    public class TagScenarios: TagScenarioBase
    {
        [Fact]
        public async Task ShouldSaveTag()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveTagCommand.Request, SaveTagCommand.Response>(Post.Tags, new SaveTagCommand.Request() {
                        Tag = new TagApiModel()
                        {
                            Name = "C#"
                        }
                    });

                Assert.True(response.TagId == 3);
            }
        }

        [Fact]
        public async Task ShouldGetAllTags()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetTagsQuery.Response>(Get.Tags);

                Assert.True(response.Tags.Count() == 2);
            }
        }
        
        [Fact]
        public async Task ShouldGetTagById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetTagByIdQuery.Response>(Get.TagById(1));

                Assert.True(response.Tag.TagId == 1);
                Assert.True(response.Tag.Name == "Angular");
            }
        }
        
        [Fact]
        public async Task ShouldUpdateTag()
        {
            using (var server = CreateServer())
            {
                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveTagCommand.Request, SaveTagCommand.Response>(Post.Tags, new SaveTagCommand.Request()
                    {
                        Tag = new TagApiModel() { TagId = 1, Name = "Angular 6" }
                    });

                Assert.True(saveResponse.TagId == 1);
            }
        }
        
        [Fact]
        public async Task ShouldDeleteTag()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Tag(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
