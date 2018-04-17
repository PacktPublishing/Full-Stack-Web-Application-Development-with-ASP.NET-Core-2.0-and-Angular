using Macaria.API.Features.Tags;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestUtilities.Extensions;
using Xunit;

namespace IntegrationTests.Features.Tags
{
    public class TagScenarios: TagScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
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

                Assert.True(response.TagId != default(int));
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetTagsQuery.Response>(Get.Tags);

                Assert.True(response.Tags.Count() > 0);
            }
        }


        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetTagByIdQuery.Response>(Get.TagById(1));

                Assert.True(response.Tag.TagId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetTagByIdQuery.Response>(Get.TagById(1));

                Assert.True(getByIdResponse.Tag.TagId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveTagCommand.Request, SaveTagCommand.Response>(Post.Tags, new SaveTagCommand.Request()
                    {
                        Tag = getByIdResponse.Tag
                    });

                Assert.True(saveResponse.TagId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
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
