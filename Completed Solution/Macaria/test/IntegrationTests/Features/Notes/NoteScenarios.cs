using Macaria.API.Features.Notes;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TestUtilities.Extensions;
using Xunit;

namespace IntegrationTests.Features.Notes
{
    public class NoteScenarios: NoteScenarioBase
    {

        [Fact]
        public async Task ShouldSave()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request() {
                        Note = new NoteApiModel()
                        {
                            Title = "First Note",
                            Body = "<p>Something Important</p>"
                        }
                    });

                Assert.True(response.NoteId != default(int));
            }
        }

        [Fact]
        public async Task ShouldAddTag()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .PostAsAsync<AddTagCommand.Request, Task>(Post.AddTag(1,1), new AddTagCommand.Request()
                    {
                        TagId = 1,
                        NoteId = 1
                    });
                
            }
        }

        [Fact]
        public async Task ShouldRemoveTag()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.RemoveTag(1,1));                
            }
        }

        [Fact]
        public async Task ShouldGetAll()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetNotesQuery.Response>(Get.Notes);

                Assert.True(response.Notes.Count() > 0);
            }
        }
        
        [Fact]
        public async Task ShouldGetById()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.NoteById(1));

                Assert.True(response.Note.NoteId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldUpdate()
        {
            using (var server = CreateServer())
            {
                var getByIdResponse = await server.CreateClient()
                    .GetAsync<GetNoteByIdQuery.Response>(Get.NoteById(1));

                Assert.True(getByIdResponse.Note.NoteId != default(int));

                var saveResponse = await server.CreateClient()
                    .PostAsAsync<SaveNoteCommand.Request, SaveNoteCommand.Response>(Post.Notes, new SaveNoteCommand.Request()
                    {
                        Note = getByIdResponse.Note
                    });

                Assert.True(saveResponse.NoteId != default(int));
            }
        }
        
        [Fact]
        public async Task ShouldDelete()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .DeleteAsync(Delete.Note(1));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
