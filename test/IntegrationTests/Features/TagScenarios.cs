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
                
                await hubConnection.StartAsync();

                hubConnection.On<dynamic>("events", (result) =>
                {
                    Assert.Equal("TagSaved", $"{result.type}");
                    Assert.NotEqual(default(Guid), new Guid($"{result.payload.tagId}"));
                    tcs.SetResult(true);
                });

                var response = await server.CreateClient()
                    .PostAsAsync<SaveTagCommand.Request, SaveTagCommand.Response>(Post.Tags, new SaveTagCommand.Request() {
                        Tag = new TagDto()
                        {
                            Name = "C#"
                        }
                    });
                
                Assert.NotEqual(default(Guid), response.TagId);
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
                var tag = (await server.CreateClient()
                    .GetAsync<GetTagsQuery.Response>(Get.Tags)).Tags.First();

                var response = await server.CreateClient()
                    .GetAsync<GetTagByIdQuery.Response>(Get.TagById(tag.TagId));

                Assert.True(response.Tag.TagId == tag.TagId);
                Assert.True(response.Tag.Name == tag.Name);
            }
        }
        
        [Fact]
        public async Task ShouldUpdateTag()
        {
            using (var server = CreateServer())
            {
                var tag = (await server.CreateClient()
                    .GetAsync<GetTagsQuery.Response>(Get.Tags)).Tags.First();

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveTagCommand.Request, SaveTagCommand.Response>(Post.Tags, new SaveTagCommand.Request()
                    {
                        Tag = new TagDto() { TagId = tag.TagId, Name = "Angular 6" }
                    });

                Assert.True(saveResponse.TagId == tag.TagId);
            }
        }
        
        [Fact]
        public async Task ShouldDeleteTag()
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (var server = CreateServer())
            {
                var hubConnection = GetHubConnection(server.CreateHandler());

                hubConnection.On<dynamic>("events", (result) =>
                {
                    Assert.Equal("TagRemoved", $"{result.type}");
                    Assert.NotEqual(default(Guid), new Guid($"{result.payload.tagId}"));
                    tcs.SetResult(true);
                });

                await hubConnection.StartAsync();

                var tag = (await server.CreateClient()
                    .GetAsync<GetTagsQuery.Response>(Get.Tags)).Tags.First();

                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Tag(tag.TagId));

                response.EnsureSuccessStatusCode();
            }

            await tcs.Task;
        }
    }
}
