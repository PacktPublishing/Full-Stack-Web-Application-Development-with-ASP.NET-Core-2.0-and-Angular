using Macaria.API.Features.Tags;
using Macaria.Core.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Features
{
    public class TagScenarios: TagScenarioBase
    {
        [Fact]
        public async Task ShouldSaveTag()
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (var server = CreateServer())
            {
                var hubConnection = GetHubConnection(server.CreateHandler());

                hubConnection.On<dynamic>("message", (result) =>
                {
                    Assert.Equal("[Tag] Saved", $"{result.type}");
                    Assert.Equal(3, Convert.ToInt16(result.payload.tag.tagId));
                    tcs.SetResult(true);
                });

                await hubConnection.StartAsync();

                var response = await server.CreateClient()
                    .PostAsAsync<SaveTagCommand.Request, SaveTagCommand.Response>(Post.Tags, new SaveTagCommand.Request() {
                        Tag = new TagApiModel()
                        {
                            Name = "C#"
                        }
                    });

                Assert.True(response.TagId == 3);
            }

            await tcs.Task;
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
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (var server = CreateServer())
            {
                var hubConnection = GetHubConnection(server.CreateHandler());

                hubConnection.On<dynamic>("message", (result) =>
                {
                    Assert.Equal("[Tag] Removed", $"{result.type}");
                    Assert.Equal(1, Convert.ToInt16(result.payload.tagId));
                    tcs.SetResult(true);
                });

                await hubConnection.StartAsync();
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Tag(1));

                response.EnsureSuccessStatusCode();
            }

            await tcs.Task;
        }
    }
}
